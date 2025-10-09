using API.Dtos.Shopping;
using API.Events;
using API.ServiceInterfaces;
using AutoMapper;
using Core.Domain.RepositoryInterfaces;
using Core.Domain.Shopping;
using FluentResults;
using NATS.Client;
using System.Text;
using System.Text.Json;

namespace Core.UseCases;

public class PurchaseService : IPurchaseService
{
    private readonly IPurchaseRepository purchaseRepository;
    private readonly IShoppingCartService shoppingCartService;
    private readonly ITourRepository tourRepository;
    private readonly IPurchaseMemoryStore purchaseMemoryStore;
    private readonly IMapper mapper;
    private readonly IConnection nats;

    public PurchaseService(
        IPurchaseRepository purchaseRepository,
        IShoppingCartService shoppingCartService,
        ITourRepository tourRepository,
        IPurchaseMemoryStore purchaseMemoryStore,
        IMapper mapper,
        IConnection nats)
    {
        this.purchaseRepository = purchaseRepository;
        this.shoppingCartService = shoppingCartService;
        this.tourRepository = tourRepository;
        this.purchaseMemoryStore = purchaseMemoryStore;
        this.mapper = mapper;
        this.nats = nats;
    }

    public async Task<Result<IEnumerable<TourPurchaseTokenDto>>> CreateAsync(long userId)
    {
        var cartDto = await shoppingCartService.GetShoppingCartByUserIdAsync(userId);
        if (cartDto is null || !cartDto.Items.Any())
            return Result.Fail("Shopping cart is empty");

        CvtCartItemDtosToTokens(cartDto.Items, userId, out var tokens);

        purchaseRepository.CreateBatch(tokens);
        await shoppingCartService.ClearCartAsync(userId);
        await purchaseRepository.SaveChangesAsync();

        foreach (var token in tokens)
        {
            var item = cartDto.Items.FirstOrDefault(i => i.TourId == token.TourId);
            if (item != null)
                purchaseMemoryStore.Save(token.Id, item);

            var tour = await tourRepository.GetByIdAsync(token.TourId);
            if (tour is null)
                return Result.Fail($"Tour with ID {token.TourId} not found, how do you think to buy this?");
            if (!long.TryParse(tour.AuthorId, out long authorId))
                return Result.Fail("Invalid user ID.");

            var evt = new TourPurchaseRequested(token.Id,token.UserId,token.TourId, authorId);
            var payload = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(evt));
            nats.Publish("tours.purchase.requested", payload);
        }

        return Result.Ok(mapper.Map<IEnumerable<TourPurchaseTokenDto>>(tokens));
    }

    private void CvtCartItemDtosToTokens(
        IEnumerable<ShoppingCartItemDto> items,
        long userId,
        out ICollection<TourPurchaseToken> tokens)
    {
        tokens = [];

        foreach (var item in items)
        {
            tokens.Add(new TourPurchaseToken
            {
                TourId = item.TourId,
                UserId = userId,
                Status = "Pending",
                IdentityValidated = false,
                FollowerValidated = false,
                RejectReason = null
            });
        }
    }
}
