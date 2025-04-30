using Galaxy_Auction_Business.Abstraction;
using Galaxy_Auction_Business.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Galaxy_Auction.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> CreateUser([FromBody] RegisterRequestDto model)
    {
        var restopnse = await _userService.Register(model);
        if(restopnse.isSuccess)
        {
            return Ok(restopnse);
        }
        return BadRequest(restopnse);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginRequestDto model)
    {
        var response = await _userService.Login(model);
        if (response.isSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

}
