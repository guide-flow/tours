namespace API.Dtos.Shopping;

public record TourPurchaseTokenDto(
    long Id,
    int TourId,
    long UserId);
