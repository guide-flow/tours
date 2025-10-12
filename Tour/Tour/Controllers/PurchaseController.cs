using API.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Tour.Controllers;

[Route("api/purchases")]
[ApiController]
public class PurchaseController : ControllerBase
{
    private readonly IPurchaseService purchaseService;

    public PurchaseController(IPurchaseService purchaseService)
    {
        this.purchaseService = purchaseService;
    }

    [HttpPost]
    public async Task<IActionResult> Create()
    {
        string userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        if (!long.TryParse(userIdStr, out long userId))
            return BadRequest("Invalid user ID.");

        var result = await purchaseService.CreateAsync(userId);
        if (result.IsFailed)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }
}
