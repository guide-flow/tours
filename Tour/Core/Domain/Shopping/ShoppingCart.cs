namespace Core.Domain.Shopping;

public class ShoppingCart
{
    private readonly ICollection<ShoppingCartItem> items = new List<ShoppingCartItem>();

    public long Id { get; init; }

    public long UserId { get; init; }

    public IReadOnlyCollection<ShoppingCartItem> Items => [.. items];

    public void AddToCart(ShoppingCartItem item)
    {
        if (items.Any(i => i.TourId == item.TourId))
            throw new InvalidOperationException("Item already in cart");

        items.Add(item);
    }
}
