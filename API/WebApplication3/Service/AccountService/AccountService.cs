using AutoMapper;
using BCrypt.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using WebApplication3.DTOs.Account;
using WebApplication3.DTOs.Auth;
using WebApplication3.DTOs.Member;
using WebApplication3.DTOs.Otp;
using WebApplication3.Helper.Data;
using WebApplication3.repository.AccountRepository;
using WebApplication3.repository.MemberRepository;
using static WebApplication3.DTOs.Auth.ServiceResponses;


namespace WebApplication3.Service.AccountService
{
    public interface IAccountService
    {
        Task<List<AccountDTO>> GetAllAcc();
        Task<AccountDTO> GetAccountsAsync(int id);

        Task<AccountDTO> GetAccountsByUserName(string username);

        Task UpdatePasswordAcc(string username, UpdatePasswordRequestDTO acc);
        Task DeleteAccount(string username);

        Task<Task> ForgotPassword(string email);

        Task EnterOtp(string otp);
        Task SendEmailAsync(string email, string subject, string messager);

    }
    public class AccountService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,IAccountRepository _AccountRepository, IMapper _mapper) : IAccountService
    {
        public async Task DeleteAccount(string username)
        {

            var acc = await _AccountRepository.GetAccountsByUserName(username);

            if (acc != null)
            {
                
                await _AccountRepository.DeleteAccount(acc);
            }
        }

        public Task SendEmailAsync(string email, string subject, string messager)
        {
            var mail = "2051063514@e.tlu.edu.vn";
            var pw = "Mangcut11";

            var client = new SmtpClient("smtp-mail.outlook.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail,pw)
            };
            return client.SendMailAsync(new MailMessage(from: mail, to: email, subject, messager));
        }

        public async Task<AccountDTO> GetAccountsAsync(int id)
        {
            var Account = await _AccountRepository.GetAccounts(id);

            if (Account == null)
                throw new KeyNotFoundException("Account not found");

            return Account;
        }

        public async Task<AccountDTO> GetAccountsByUserName(string username)
        {
            var Account = await _AccountRepository.GetAccountsByUserName(username);

            if (Account == null)
                throw new KeyNotFoundException("Account not found");

            return Account;
        }

        public async Task<List<AccountDTO>> GetAllAcc()
        {
            return await _AccountRepository.GetAllAcc();
        }

        public async Task UpdatePasswordAcc(string username, UpdatePasswordRequestDTO acc)
        {
            var user = await _AccountRepository.GetAccountsByUserName(username);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            // copy model props to user
            _mapper.Map(acc, user);
            var getUser = await userManager.FindByNameAsync(username);
            await userManager.ChangePasswordAsync(getUser, getUser.PasswordHash, acc.Password);
            // save user
            await _AccountRepository.UpdatePasswordAcc(user);
        }

        public async Task<Task> ForgotPassword(string email)
        {
            var otp = GenerateOTP();
              saveOtp(otp,email);
            return SendEmailAsync(email, "lấy lại mật khẩu", "Để lấy lại mật khẩu vui lòng dùng OTP này: "+ otp);

        }
        public async Task EnterOtp(string otp)
        {
            var vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // Mã này tương ứng với GMT+7
            var vnTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTimeZone);
            var OTP = await _AccountRepository.GetOtp(otp);
            if (OTP == null){
                throw new KeyNotFoundException("Otp bị sai, mời nhập lại");
             }
            else if (OTP.expires_at < vnTime) { 
                throw new Exception("OTP đã hết hạn"); 
            }
            else
            {
                OTP.IsVerified = true;
                await _AccountRepository.UpdateOtp(OTP);

            }

        }
        private static string GenerateOTP()
        {
            var otp = "";
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[6];
                rng.GetBytes(randomNumber);
                otp = string.Concat(randomNumber.Select(b => b % 10));
            }

            // Đảm bảo mã OTP luôn có đúng 6 ký tự
            if (otp.Length < 6)
            {
                otp = otp.PadRight(6, '0');
            }
            else if (otp.Length > 6)
            {
                otp = otp.Substring(0, 6);
            }

            return otp;
        }
        private async Task saveOtp(string otp,string email)
        {
            await _AccountRepository.SaveOtp(otp, email);
        }

    }
}
   