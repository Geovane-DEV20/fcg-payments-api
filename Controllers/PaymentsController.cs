using Microsoft.AspNetCore.Mvc;

namespace FcgPaymentsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { message = "Payments API is running" });
    }
}
