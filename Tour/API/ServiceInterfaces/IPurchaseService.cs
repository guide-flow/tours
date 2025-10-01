using API.Dtos.Shopping;
using FluentResults;

namespace API.ServiceInterfaces;

public interface IPurchaseService
{
    /// <summary>
    /// Essentially acts as a "checkout" function, creating a purchase token for each item in the user's cart
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<Result<IEnumerable<TourPurchaseTokenDto>>> CreateAsync(long userId);
}
