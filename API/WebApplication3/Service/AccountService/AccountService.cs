using AutoMapper;
using BCrypt.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
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
using MailKit.Net.Smtp;
using MimeKit;

namespace WebApplication3.Service.AccountService
{
    public interface IAccountService
    {
        Task<List<AccountDTO>> GetAllAcc();
        Task<AccountDTO> GetAccountsAsync(int id);

        Task<AccountDTO> GetAccountsByUserName(string username);

        Task UpdatePasswordAcc(string username, UpdatePasswordRequestDTO acc);
        Task DeleteAccount(string username);

        Task ForgotPassword(string email);

        Task EnterOtp(string otp);
        Task SendEmailAsync(string email, string subject, string messager);

        Task changeForgetPass(string email, string newPass, string otp);

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

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = "2051063514@e.tlu.edu.vn";
            var pw = "Mangcut11";

            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("Your Name", mail)); // Chú ý thay thế "Your Name" bằng tên hiển thị mong muốn
            mimeMessage.To.Add(new MailboxAddress("Recipient", email)); // Chú ý thay thế "Recipient" bằng tên hiển thị mong muốn
            mimeMessage.Subject = subject;
            mimeMessage.Body = new TextPart("plain")
            {
                Text = message
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp-mail.outlook.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(mail, pw);
                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);
            }
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

        public async Task ForgotPassword(string email)
        {
            var acc = await _AccountRepository.GetAccountByEmail(email);
            if (acc == null)
            {
                throw new KeyNotFoundException("Email không tồn tại");
            }

            var otp = GenerateOTP();
             _AccountRepository.SaveOtp(otp, email);

              SendEmailAsync(email, "Lấy lại mật khẩu", $"Để lấy lại mật khẩu vui lòng dùng OTP này: {otp}");
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
            else if (OTP.IsVerified == true)
            {
                throw new Exception("OTP đã được xác nhận");
            }

        }
        private static string GenerateOTP()
        {
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                var bytes = new byte[4]; // Tạo mảng byte đủ để lấy số ngẫu nhiên
                rng.GetBytes(bytes);
                var num = BitConverter.ToUInt32(bytes, 0);
                return (num % 1000000).ToString("D6"); // Đảm bảo mã OTP là 6 chữ số
            }
        }
        private async Task saveOtp(string otp,string email)
        {
            await _AccountRepository.SaveOtp(otp, email);
        }
        public async Task changeForgetPass(string email,string newPass, string otp)
        {
            var vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // Mã này tương ứng với GMT+7
            var vnTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTimeZone);
            var OTP = await _AccountRepository.GetOtp(otp);
            if (OTP == null)
            {
                throw new KeyNotFoundException("Otp bị sai, mời nhập lại");
            }
            else if (OTP.expires_at < vnTime)
            {
                throw new Exception("OTP đã hết hạn");
            }
            else if (OTP.IsVerified == true)
            {
                throw new Exception("OTP đã được xác nhận");
            }    
            else
            {
            
                var getUserIdentity = await userManager.FindByEmailAsync(email);
 
                if (getUserIdentity == null)
                {
                    throw new KeyNotFoundException("Email này không tồn tại");
                }
                if(string.IsNullOrEmpty(newPass))
                {
                    throw new Exception("Nhập thiếu mật khẩu");
                }
                var getUserDb = await _AccountRepository.GetAccountsByUserName(getUserIdentity.UserName);
                var a =await userManager.ChangePasswordAsync(getUserIdentity, getUserDb.Password, newPass);
                OTP.IsVerified = true;
                _AccountRepository.UpdateOtp(OTP);
                _AccountRepository.UpdatePasswordAccByEmail(email, newPass);
            }
        }
    }
}
   