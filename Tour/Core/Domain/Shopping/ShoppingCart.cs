namespace Core.Domain.Shopping;

public class ShoppingCart
{
    private readonly ICollection<ShoppingCartItem> items = new List<ShoppingCartItem>();

    public long Id { get; init; }

    public long UserId { get; init; }

    public decimal TotalPrice => items.Sum(i => i.Price);

    public IReadOnlyCollection<ShoppingCartItem> Items => [.. items];

    public void AddToCart(ShoppingCartItem item)
    {
        if (items.Any(i => i.TourId == item.TourId))
            throw new InvalidOperationException("Item already in cart");

        items.Add(item);
    }

    public void RemoveFromCart(int tourId)
    {
        var item = items.FirstOrDefault(i => i.TourId == tourId);
        if (item is null)
            throw new InvalidOperationException("Item not found in cart");

        items.Remove(item);
    }

    public void EmptyCart()
    {
        items.Clear();
    }
}
