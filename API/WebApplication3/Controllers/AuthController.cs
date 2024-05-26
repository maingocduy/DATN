﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.DTOs.Auth;
using WebApplication3.Entities;
using WebApplication3.repository.AccountRepository;
using WebApplication3.Service.AccountService;
using WebApplication3.Service.AuthService;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }
        [HttpPost("login")]
        public async Task<ActionResult<account>> LoginAsync(LoginDTO login)
        {
            var a = await authService.login(login);
            return Ok(a);
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Create(CreateAccountRequestDTO acc)
        {
            return Ok(await authService.RegisterNewAccount(acc));
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var logoutResult = await authService.Logout();

            if (logoutResult)
            {
                return Ok(new { message = "Logout successful." });
            }
            else
            {
                return BadRequest(new { message = "Invalid or expired JWT." });
            }
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var response = await authService.GenerateJwtTokenFromRefreshToken();
            return Ok(response);
        }
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            var result = await authService.ConfirmEmailAsync(userId, code);
            return Ok(result.Message);
        }
    }
}
