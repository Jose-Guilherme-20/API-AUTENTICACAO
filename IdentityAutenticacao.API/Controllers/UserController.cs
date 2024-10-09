using IdentityAutenticacao.API.Services.Interfaces;
using IdentityAutenticacao.API.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAutenticacao.API.Controllers;

[ApiController]
[Route("api/v1/user")]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost]
    public async Task<ActionResult> CreateUser([FromBody]CreateUserViewModel request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _userService.CreateUserAsync(request.Email, request.Password);

        if (result.Succeeded)
        {
            return Ok(new { Message = "User created successfully." });
        }

        return BadRequest(result.Errors);
    }
}