using Application.DTOs.Auth.Login;
using Application.DTOs.Auth.Password;
using Application.DTOs.Auth.Register;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]

public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto input)
    {
        var response = await _authService.LoginAsync(input);
        if (response == null)
            return Unauthorized("Invalid email or password.");

        return Ok(response);
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto input)
    {
        var response = await _authService.RegisterAsync(input);
        if (response == null)
            return BadRequest("Cannot Register.");

        return Ok(response);
    }

    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto input)
    {
        await _authService.ResetPassword(input);
        return Ok();
    }

    [Authorize(Roles = "User")]
    [HttpGet("ProfileUser")]
    public async Task<IActionResult> UserProfile()
    {
        var response = await _authService.UserProfile();
        if (response == null)
            return Unauthorized();

        return Ok(response);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("ProfileAdmin")]
    public async Task<IActionResult> AdminProfile()
    {
        var response = await _authService.AdminProfile();
        if (response == null)
            return Unauthorized();

        return Ok(response);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("getAllUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        var response = await _authService.GetAllUsers();
        return Ok(response);
    }
}
