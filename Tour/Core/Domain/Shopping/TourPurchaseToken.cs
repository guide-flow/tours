namespace Core.Domain.Shopping;

public class TourPurchaseToken
{
    public long Id { get; init; }

    public int TourId { get; init; }

    public Tour? Tour { get; private set; } = null;

    public long UserId { get; init; }

    public string Status { get; set; }
    public bool IdentityValidated { get; set; }
    public bool FollowerValidated { get; set; }
    public string? RejectReason { get; set; }

}
