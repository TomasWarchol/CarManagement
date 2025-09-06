using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarManagement.API;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class ExampleController() : ControllerBase
{

    [HttpGet]
    public Task<ActionResult<string>> Example()
    {
        return Task.FromResult<ActionResult<string>>(Ok("Example response from API"));
    }

}