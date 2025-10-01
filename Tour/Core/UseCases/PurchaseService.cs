using API.Dtos.Shopping;
using API.ServiceInterfaces;
using AutoMapper;
using Core.Domain.RepositoryInterfaces;
using Core.Domain.Shopping;
using FluentResults;

namespace Core.UseCases;

public class PurchaseService : IPurchaseService
{
    private readonly IPurchaseRepository purchaseRepository;
    private readonly IShoppingCartService shoppingCartService;
    private readonly IMapper mapper;

    public PurchaseService(
        IPurchaseRepository purchaseRepository,
        IShoppingCartService shoppingCartService,
        IMapper mapper)
    {
        this.purchaseRepository = purchaseRepository;
        this.shoppingCartService = shoppingCartService;
        this.mapper = mapper;
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
                UserId = userId
            });
        }
    }
}
