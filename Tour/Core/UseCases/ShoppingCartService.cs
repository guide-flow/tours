using API.Dtos.Shopping;
using API.ServiceInterfaces;
using AutoMapper;
using Core.Domain.RepositoryInterfaces;
using Core.Domain.Shopping;
using FluentResults;
using ItemCreationDto = API.Dtos.Shopping.ShoppingCartItemCreationDto;

namespace Core.UseCases;

public class ShoppingCartService : IShoppingCartService
{
    private readonly IShoppingCartRepository shoppingCartRepository;
    private readonly ITourService tourService;
    private readonly IMapper mapper;

    public ShoppingCartService(
        IShoppingCartRepository shoppingCartRepository,
        ITourService tourService,
        IMapper mapper)
    {
        this.shoppingCartRepository = shoppingCartRepository;
        this.tourService = tourService;
        this.mapper = mapper;
    }

    public async Task<Result<ShoppingCartItemDto>> AddToCartAsync(ItemCreationDto itemCreationDto, long userId)
    {
        // Get or create shopping cart for user. If cart does not exist, create a new one. Keep it in same transaction, hence no async method.
        var cart = await shoppingCartRepository.GetByUserIdAsync(userId) ?? shoppingCartRepository.Create(new ShoppingCart { UserId = userId });

        var tour = await tourService.GetTourByIdAsync(itemCreationDto.TourId);
        if (tour is null)
            return Result.Fail("Tour not found");

        var item = mapper.Map<ShoppingCartItem>(itemCreationDto);

        try
        {
            cart.AddToCart(item);
            await shoppingCartRepository.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return Result.Fail(e.Message);
        }

        return Result.Ok(mapper.Map<ShoppingCartItemDto>(item));
    }
}
