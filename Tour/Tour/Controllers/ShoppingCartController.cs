using API.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ItemCreationDto = API.Dtos.Shopping.ShoppingCartItemCreationDto;

namespace Tour.Controllers;

[Route("api/shopping-cart")]
[ApiController]
public class ShoppingCartController : ControllerBase
{
    private readonly IShoppingCartService shoppingCartService;

    public ShoppingCartController(IShoppingCartService shoppingCartService)
    {
        this.shoppingCartService = shoppingCartService;
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(ItemCreationDto itemCreationDto)
    {
        string userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        if (!long.TryParse(userIdStr, out long userId))
            return BadRequest("Invalid user ID.");
        var result = await shoppingCartService.AddToCartAsync(itemCreationDto, userId);
        if (result.IsFailed)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpDelete("{tourId:int:min(1)}")]
    public async Task<IActionResult> RemoveFromCart([FromRoute] int tourId)
    {
        string userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        if (!long.TryParse(userIdStr, out long userId))
            return BadRequest("Invalid user ID.");
        var result = await shoppingCartService.RemoveFromCartAsync(tourId, userId);
        if (result.IsFailed)
            return BadRequest(result.Errors);

        return NoContent();
    }
}
