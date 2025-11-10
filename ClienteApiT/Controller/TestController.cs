using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("test")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok("API funcionando correctamente");
}