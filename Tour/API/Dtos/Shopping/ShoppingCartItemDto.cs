namespace API.Dtos.Shopping;

public record ShoppingCartItemDto(long Id, long ShoppingCartId, int TourId, string TourName, decimal Price);