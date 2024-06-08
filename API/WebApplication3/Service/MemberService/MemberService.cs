using AutoMapper;
using CloudinaryDotNet.Core;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using System.Xml.Linq;
using WebApplication3.DTOs.Member;
using WebApplication3.DTOs.Sponsor;
using WebApplication3.Entities;
using WebApplication3.repository.GroupsRepository;
using WebApplication3.repository.MemberRepository;
using WebApplication3.repository.ProjectReposiotry;
using WebApplication3.repository.SponsorRepository;
using WebApplication3.Service.AccountService;

namespace WebApplication3.Service.MemberService
{
    public interface IMemberService
    {
        Task<List<MemberDTO>> GetAllMember(int? ProjectId = null, string? groupName = null);
        Task<MemberDTO> GetMemberAsync(int id);

        Task<MemberDTO> GetMember(string member);
        Task AddMember(string name, CreateRequestMemberDTO acc);
        Task UpdateMember(UpdateRequestMember update);
        Task DeleteMember(string member);

        Task JoinProject(string project_name, string member_name);

        Task EnterOtp(string otp);
    }
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository IMemberRepository;
        private readonly IMapper _mapper;
        private readonly IProjectRepository projectRepository;
        public MemberService(IMemberRepository IMemberRepository,IProjectRepository projectRepository,IAccountService accountService, IMapper mapper)
        {
            this.IMemberRepository = IMemberRepository;
            _mapper = mapper;
            this.projectRepository = projectRepository;

        }
        public async Task AddMember(string name, CreateRequestMemberDTO mem)
        {
            // Kiểm tra xem dự án có tồn tại không
            var project = await projectRepository.GetProjectID(name);
            var member = await IMemberRepository.GetMemberByEmail(mem.Email);
            if (project == null)
            {
                throw new KeyNotFoundException("Project not found");
            }
            if(member != null)
            {
                
                await IMemberRepository.JoinProject(project.Project_id,member.Member_id );
            }  
            else
            {
                var memberDTO = _mapper.Map<MemberDTO>(mem);
                var otp = GenerateOTP();
                await IMemberRepository.AddMember(project.Project_id, memberDTO);
                IMemberRepository.SaveOtp(otp, memberDTO.email);

                SendEmailAsync(memberDTO.email, "Xác Nhận Email", $"Để xác nhận Email vui lòng dùng OTP này: {otp}");

            };
            
        }
        public async Task EnterOtp(string otp)
        {
            var vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // Mã này tương ứng với GMT+7
            var vnTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTimeZone);
            var OTP = await IMemberRepository.GetOtp(otp);
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
                OTP.IsVerified = true;
                IMemberRepository.UpdateOtp(OTP);
            }

        }
        
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = "2051063514@e.tlu.edu.vn";
            var pw = "Mangcut11";

            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("Hội bác sĩ tình nguyện", mail)); // Chú ý thay thế "Your Name" bằng tên hiển thị mong muốn
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
        public async Task DeleteMember(string name)
        {
          
            await IMemberRepository.DeleteMember(name);
            
        }

        public async Task<List<MemberDTO>> GetAllMember(int? ProjectId = null, string? groupName = null)
        {
            return await IMemberRepository.GetAllMember(ProjectId, groupName);
        }

        public async Task<MemberDTO> GetMember(string member)
        {
            var group = await IMemberRepository.GetMember(member);

            if (group == null)
                throw new KeyNotFoundException("member not found");

            return group;
        }

        public Task<MemberDTO> GetMemberAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task JoinProject(string project_name, string member_name)
        {
            var member = await IMemberRepository.GetMember(member_name);

            var project = await projectRepository.GetProject(project_name);

            if (member== null || project == null)
            {
                throw new Exception($"Member or project does not exist");
            }

            await IMemberRepository.JoinProject(project.Project_id, member.Member_id);
        }

        public async Task UpdateMember(UpdateRequestMember update)
        {
            // Kiểm tra xem thành viên có tồn tại không
            var existingMember = await IMemberRepository.GetMember(update.Name);
            if (existingMember == null)
            {
                throw new Exception($"Member with the name '{update.Name}' does not exist");
            }

            // Kiểm tra xem email mới đã tồn tại cho một thành viên khác chưa
            var existingMemberByEmail = await IMemberRepository.GetMemberByEmail(update.Email);
            if (existingMemberByEmail != null && existingMemberByEmail.email != existingMember.email)
            {
                throw new Exception($"Member with the email '{update.Email}' already exists for another member");
            }

            // Cập nhật thông tin thành viên
            existingMember.name = update.Name;
            existingMember.phone = update.Phone;
            existingMember.email = update.Email;

            // Gọi repository để cập nhật thông tin thành viên
            await IMemberRepository.UpdateMember(existingMember);
        }


    }
}
