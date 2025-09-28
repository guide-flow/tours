using API.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

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
        var result = await shoppingCartService.AddToCartAsync(itemCreationDto, 1); // TODO: Replace 1 with actual user id from auth context
        if (result.IsFailed)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }
}
