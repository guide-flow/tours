namespace API.Dtos.Shopping;

public record ShoppingCartDto(
    long Id,
    long UserId,
    decimal TotalPrice,
    IEnumerable<ShoppingCartItemDto> Items);
