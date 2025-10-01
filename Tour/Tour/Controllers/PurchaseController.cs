using API.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

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
        var result = await purchaseService.CreateAsync(1); // TODO: Replace with actual user ID
        if (result.IsFailed)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }
}
