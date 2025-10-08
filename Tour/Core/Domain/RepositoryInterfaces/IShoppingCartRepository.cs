using Core.Domain.Shopping;

namespace Core.Domain.RepositoryInterfaces;

public interface IShoppingCartRepository
{
    Task SaveChangesAsync();

    Task<ShoppingCart?> GetByUserIdAsync(long userId);

    /// <summary>
    /// Adds a new cart to the context
    /// </summary>
    /// <param name="cart"></param>
    /// <returns></returns>
    ShoppingCart Create(ShoppingCart cart);

    /// <summary>
    /// Adds a new cart to the context and saves changes to the database
    /// </summary>
    /// <param name="cart"></param>
    /// <returns></returns>
    Task<ShoppingCart> CreateAsync(ShoppingCart cart);
}
