using Core.Domain.RepositoryInterfaces;
using Core.Domain.Shopping;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories;

public class PurchaseRepository : IPurchaseRepository
{
    private readonly ToursContext context;

    public PurchaseRepository(ToursContext context)
    {
        this.context = context;
    }

    public void CreateBatch(IEnumerable<TourPurchaseToken> tokens)
    {
        context.TourPurchaseTokens.AddRangeAsync(tokens);
    }

    public async Task CreateBatchAsync(IEnumerable<TourPurchaseToken> tokens)
    {
        await context.TourPurchaseTokens.AddRangeAsync(tokens);
        await context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task<TourPurchaseToken?> GetByIdAsync(long id)
    {
        return await context.TourPurchaseTokens.FirstOrDefaultAsync(t => t.Id == id);
    }
}
