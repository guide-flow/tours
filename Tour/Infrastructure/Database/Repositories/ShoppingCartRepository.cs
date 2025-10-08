using Core.Domain.RepositoryInterfaces;
using Core.Domain.Shopping;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories;

public class ShoppingCartRepository : IShoppingCartRepository
{
    private readonly ToursContext context;
    private readonly DbSet<ShoppingCart> carts;

    public ShoppingCartRepository(ToursContext context)
    {
        this.context = context;
        carts = context.Set<ShoppingCart>();
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
    public async Task<ShoppingCart?> GetByUserIdAsync(long userId)
    {
        return await carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public ShoppingCart Create(ShoppingCart cart)
    {
        carts.Add(cart);
        return cart;
    }

    public async Task<ShoppingCart> CreateAsync(ShoppingCart cart)
    {
        carts.Add(cart);
        await context.SaveChangesAsync();
        return cart;
    }
}
