using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.DTOs.Account;
using WebApplication3.DTOs.Auth;
using WebApplication3.DTOs.Otp;
using WebApplication3.Entities;
using WebApplication3.Service.AccountService;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AccountController : Controller
    {
        private readonly IAccountService accountRepository;
        public AccountController(IAccountService accountRepository)
        {
            this.accountRepository = accountRepository;
        }
        [HttpGet, Authorize(Roles ="Admin")]
        public async Task<ActionResult<List<account>>> GetAllAcc()
        {
            return Ok(await accountRepository.GetAllAcc());
        }

        [HttpGet("id")]
        public async Task<ActionResult<account>> GetAccount(int id)
        {
            var account = await accountRepository.GetAccountsAsync(id);
            return Ok(account);
        }
       
        [HttpPut("{username}")]
        public async Task<IActionResult> Update(string username,UpdatePasswordRequestDTO acc)
        {
            await accountRepository.UpdatePasswordAcc(username, acc);
            return Ok(new { message = "Pass updated" });
        }

        [HttpDelete("{username}")]
        public async Task<IActionResult> Delete(string username)
        {
            await accountRepository.DeleteAccount(username);
            return Ok(new { message = "User deleted" });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(string token)
        {
           

            // Trả về một kết quả thành công hoặc thông tin khác tùy thuộc vào logic của bạn
            return Ok("Logout");
        }
        [HttpPost("forgot")]
        public async Task<IActionResult> ForgotPass([FromBody]  ForgetPassDTO forgetPass)
        {
            try { 
                await accountRepository.ForgotPassword(forgetPass.email);
                return Ok(new { Message = "Gửi thành công." });            
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPost("enter_otp")]
        public async Task<IActionResult> EnterOtp([FromBody]  otpRequest request)
        {
            try
            {
                await accountRepository.EnterOtp(request.Otp);
                return Ok("Thành công");
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        [HttpPost("changeForgetPass")]
        public async Task<IActionResult> ChangeForgetPass(EnterPassRequest request)
        {
            try
            {
                await accountRepository.changeForgetPass(request.Email, request.Password, request.Otp);
                return Ok(new { Message = "Password changed successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
