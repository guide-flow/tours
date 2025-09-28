using API.Dtos.Shopping;
using FluentResults;

namespace API.ServiceInterfaces;

public interface IShoppingCartService
{
    Task<Result<ShoppingCartItemDto>> AddToCartAsync(ShoppingCartItemCreationDto itemCreationDto, long userId);
}
