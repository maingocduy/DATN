using CloudinaryDotNet.Core;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApplication3.DTOs.Account;
using WebApplication3.DTOs.Groups;
using WebApplication3.DTOs.Member;
using WebApplication3.DTOs.Otp;
using WebApplication3.Entities;
using WebApplication3.Helper.Data;

namespace WebApplication3.repository.MemberRepository
{
    public interface IMemberRepository
    {
        Task<OtpDTO> GetOtp(string otp);
        Task DeleteOtp(string otp);
        Task<List<MemberDTO>> GetAllMember(int? ProjectId = null, string? GroupName = null);
        Task AddMember(int project_id, MemberDTO mem);
        Task DeleteMember(string name);
        Task<MemberDTO> GetMember(string name);
        Task UpdateMember(MemberDTO member);
        Task<int> AddNewMember(MemberDTO memberDTO);
        Task<MemberDTO> GetMemberByEmail(string email);
        Task JoinProject(int project_id, int member_id);
        Task<MemberDTO> GetMemberById(int id);
        Task<int> getIDMember(string name);
        Task SaveOtp(string otp, string emailMember);
        Task UpdateOtp(OtpDTO otp);
    }
    public class MemberRepository : IMemberRepository
    {
        private AppDbContext _context;
        public MemberRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddMember(int project_id, MemberDTO memberDTO)
        {
            using var connection = _context.CreateConnection();

            var sqlAddMember = @"
INSERT INTO Members (name, email, phone, Group_id)
VALUES (@Name, @Email, @Phone, (SELECT * FROM `groups` WHERE group_name = @group_name));
SELECT LAST_INSERT_ID();";

            // Thực hiện thêm thành viên và lấy member_id của thành viên mới thêm vào
            var member_id = await connection.ExecuteScalarAsync<int>(sqlAddMember, new
            {
                Name = memberDTO.name,
                Email = memberDTO.email,
                Phone = memberDTO.phone,
                group_name = memberDTO.groups.group_name
            });
            // Thêm vào bảng MemberProjects
            var sqlAddToProject = @"INSERT INTO MemberProjects (Member_id, Project_id) VALUES (@member_id, @project_id);";
            await connection.ExecuteAsync(sqlAddToProject, new
            {
                member_id = member_id,
                project_id = project_id
            });
        }
        public async Task AddToNewProject(int project_id, int member_id)
        {
            using var connection = _context.CreateConnection();

         
            var sqlAddToProject = @"INSERT INTO MemberProjects (Member_id, Project_id) VALUES (@member_id, @project_id);";
            await connection.ExecuteAsync(sqlAddToProject, new
            {
                member_id = member_id,
                project_id = project_id
            });
        }
        public async Task<MemberDTO> GetMemberById(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = @"Select * from Members WHERE Member_id = @id";
            var member= await connection.QueryAsync<MemberDTO>(sql, new { id });
            return member.FirstOrDefault();
        }
        public async Task<int> AddNewMember(MemberDTO memberDTO)
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = @"
            INSERT INTO Members (name, email, phone, Group_id)
            VALUES (@Name, @Email, @Phone, (SELECT Group_id from `groups` where group_name = @group_name));
            SELECT LAST_INSERT_ID();";

                 return await connection.ExecuteScalarAsync<int>(sql, new
                {
                    Name = memberDTO.name,
                    Email = memberDTO.email,
                    Phone = memberDTO.phone,
                    group_name = memberDTO.groups.group_name
                });
            }
        }

        public async Task DeleteMember(string name)
        {
            using var connection = _context.CreateConnection();
            var deleteMemberProjectsSql = @"
        DELETE FROM MemberProjects
        WHERE Member_id IN (
            SELECT Member_id FROM Members WHERE Name = @name
        );";

            var deleteAccSql = @"
        DELETE FROM account
        WHERE Member_id IN (
            SELECT Member_id FROM Members WHERE Name = @name
        );";

            var deleteOtpSql = @"
        DELETE FROM otp_Member
        WHERE Member_id IN (
            SELECT Member_id FROM Members WHERE Name = @name
        );";
       

            // Tạo các task cho mỗi truy vấn xóa
            var task1 = connection.ExecuteAsync(deleteMemberProjectsSql, new { name });
            var task2 = connection.ExecuteAsync(deleteAccSql, new { name });
            var task3 = connection.ExecuteAsync(deleteOtpSql, new { name });

            // Chạy các task xóa cùng lúc
            await Task.WhenAll(task1, task2, task3);

            var deleteProjectSql = @"
        DELETE FROM Members
        WHERE Name = @name;";

            // Sau khi tất cả các task xóa đã hoàn thành, tiến hành xóa thành viên
            await connection.ExecuteAsync(deleteProjectSql, new { name });

        }

        public async Task<List<MemberDTO>> GetAllMember(int? ProjectId = null, string? GroupName = null)
        {
            using var connection = _context.CreateConnection();

            // SQL query using multi-mapping
            var dtoSql = @"
        SELECT m.*, g.*
        FROM Members AS m
        JOIN `Groups` AS g ON m.Group_id = g.Group_id";

            // List of conditions for dynamic query
            var conditions = new List<string>();

            if (ProjectId.HasValue)
            {
                conditions.Add("m.Member_id IN (SELECT Member_id FROM MemberProjects WHERE Project_id = @ProjectId)");
            }

            if (!string.IsNullOrWhiteSpace(GroupName))
            {
                conditions.Add("g.Group_name = @GroupName");
            }

            // Append conditions to SQL query if any
            if (conditions.Any())
            {
                dtoSql += " WHERE " + string.Join(" AND ", conditions);
            }

            // Perform query with Dapper
            var members = await connection.QueryAsync<MemberDTO, Group, MemberDTO>(
                dtoSql,
                (member, group) =>
                {
                    member.groups = group; // Assuming MemberDTO has a property 'Group' of type 'Group'
                    return member; // Returns the member with associated group
                },
                param: new { ProjectId = ProjectId, GroupName = GroupName },
                splitOn: "Group_id");

            return members.ToList();
        }

        public async Task<MemberDTO> GetMember(string name)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
    SELECT m.*, g.*
    FROM Members AS m
    JOIN `Groups` AS g ON m.Group_id = g.Group_id 
    WHERE m.name = @name";
            var member = await connection.QueryAsync<MemberDTO, Group, MemberDTO>(
                sql,
                (member, group) =>
                {
                    member.groups = group;
                    return member;
                },
                new { name },
                splitOn: "Group_id"
            );
            return member.FirstOrDefault();
        }

        public async Task<MemberDTO> GetMemberByEmail(string email)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
    SELECT m.*, g.*
    FROM Members AS m
    JOIN `Groups` AS g ON m.Group_id = g.Group_id 
    WHERE m.email = @email";
            var member = await connection.QueryAsync<MemberDTO, Group, MemberDTO>(
                sql,
                (member, group) =>
                {
                    member.groups = group;
                    return member;
                },
                new { email },
                splitOn: "Group_id"
            );
            return member.FirstOrDefault();
        }

        public async Task UpdateMember(MemberDTO member)
        {
            using var connection = _context.CreateConnection();

            var sql = @"
        UPDATE Members
        SET Name = @Name,
            Email = @Email,
            Phone = @Phone
        WHERE Member_id = @Member_id;";

            await connection.ExecuteAsync(sql, member);
        }
        public async Task JoinProject(int project_id,int member_id)
        {
            using var connection = _context.CreateConnection();

            var sql = @"
         INSERT INTO MemberProjects (Member_id, Project_id)
        VALUES (@member_id, @project_id);";

            await connection.ExecuteAsync(sql, new
            {
                member_id = member_id,
                project_id = project_id
            });
        }

        public async Task<int> getIDMember(string name)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
            SELECT Member_id
            FROM Members
            WHERE Name = @name";

            var memberId = await connection.QueryFirstOrDefaultAsync<int>(sql, new { name });

            return memberId;
        }
        public async Task SaveOtp(string otp, string emailMember)
        {
            using var connection = _context.CreateConnection();
            var sql = """
            INSERT INTO otp_Member (otp_code, created_at,expires_at,Member_id)
            VALUES (@otp, @created_at,@expires_at,(Select Member_id from Members where email = @email))
        """;
            var vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // Mã này tương ứng với GMT+7
            var vnTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTimeZone);
            await connection.ExecuteAsync(sql, new
            {
                otp = otp,
                created_at = vnTime,
                expires_at = vnTime.AddMinutes(5),
                email = emailMember
            });
        }
        public async Task UpdateOtp(OtpDTO otp)
        {
            using var connection = _context.CreateConnection();
            var sql = """
        UPDATE otp_Member
        SET IsVerified = @IsVerified
        WHERE otp_code = @OtpCode
        """;
            await connection.ExecuteAsync(sql, new
            {
                IsVerified = otp.IsVerified,
                OtpCode = otp.otp_code
            });
        }
        public async Task<OtpDTO> GetOtp(string otp)
        {
            using var connection = _context.CreateConnection();
            var sql = """
    SELECT *
           FROM otp_Member
           WHERE otp_code = @otp
    """;
            var acc = await connection.QueryAsync<OtpDTO>(sql, new { otp });
            return acc.FirstOrDefault();
        }
        public async Task DeleteOtp(string otp)
        {
            using var connection = _context.CreateConnection();
            var sql = """
            Delete 
        			FROM otp_Member
        			
            WHERE otp_code = @otp
        """;
            await connection.QueryAsync(sql, new { otp });

        }
    }
}
