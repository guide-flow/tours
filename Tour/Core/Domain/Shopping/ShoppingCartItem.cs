using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Shopping;

public class ShoppingCartItem
{
    public long Id { get; init; }

    public long ShoppingCartId { get; init; }

    public int TourId { get; init; }

    [Required]
    [MaxLength(200)]
    public string TourName { get; private set; } = string.Empty;

    /// <summary>
    /// Price snapshot. Price at the time of adding to cart
    /// </summary>
    [Required]
    public decimal Price { get; private set; }
}
