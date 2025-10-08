namespace Core.Domain.Shopping;

public class TourPurchaseToken
{
    public long Id { get; init; }

    public int TourId { get; init; }

    public Tour? Tour { get; private set; } = null;

    public long UserId { get; init; }
}
