using Core.Domain.Shopping;

namespace Core.Domain.RepositoryInterfaces;

public interface IPurchaseRepository
{
    void CreateBatch(IEnumerable<TourPurchaseToken> tokens);

    Task CreateBatchAsync(IEnumerable<TourPurchaseToken> tokens);

    Task SaveChangesAsync();
}
