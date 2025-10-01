using API.Dtos.Shopping;
using FluentResults;

namespace API.ServiceInterfaces;

public interface IShoppingCartService
{
    Task<Result<ShoppingCartItemDto>> AddToCartAsync(ShoppingCartItemCreationDto itemCreationDto, long userId);

    Task<Result> RemoveFromCartAsync(int tourId, long userId);

    Task<ShoppingCartDto?> GetShoppingCartByUserIdAsync(long userId);

    Task ClearCartAsync(long userId);
}
