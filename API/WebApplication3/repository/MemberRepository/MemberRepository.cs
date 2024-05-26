using CloudinaryDotNet.Core;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApplication3.DTOs.Account;
using WebApplication3.DTOs.Groups;
using WebApplication3.DTOs.Member;
using WebApplication3.Helper.Data;

namespace WebApplication3.repository.MemberRepository
{
    public interface IMemberRepository
    {
        Task<List<MemberDTO>> GetAllMember();
        Task AddMember(int project_id, MemberDTO mem);
        Task DeleteMember(string name);
        Task<MemberDTO> GetMember(string name);
        Task UpdateMember(MemberDTO member);
        Task<int> AddNewMember(MemberDTO memberDTO);
        Task<MemberDTO> GetMemberByEmail(string email);
        Task JoinProject(int project_id, int member_id);
        Task<MemberDTO> GetMemberById(int id);
        Task<int> getIDMember(string name);
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
VALUES (@Name, @Email, @Phone, @Group_id);
SELECT LAST_INSERT_ID();";

            // Thực hiện thêm thành viên và lấy member_id của thành viên mới thêm vào
            var member_id = await connection.ExecuteScalarAsync<int>(sqlAddMember, new
            {
                Name = memberDTO.name,
                Email = memberDTO.email,
                Phone = memberDTO.phone,
                Group_id = memberDTO.groups.Groups_id
            });
            // Thêm vào bảng MemberProjects
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
            VALUES (@Name, @Email, @Phone, @Group_id);
            SELECT LAST_INSERT_ID();";

                 return await connection.ExecuteScalarAsync<int>(sql, new
                {
                    Name = memberDTO.name,
                    Email = memberDTO.email,
                    Phone = memberDTO.phone,
                    Group_id = memberDTO.groups.Groups_id
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

            // Thực hiện câu lệnh SQL xóa MemberProjects
            await connection.ExecuteAsync(deleteMemberProjectsSql, new { name });
            var deleteProjectSql = @"
DELETE FROM Members
WHERE Name = @name;";

            // Thực hiện câu lệnh SQL xóa Project
            await connection.ExecuteAsync(deleteProjectSql, new {name });

        }

        public async Task<List<MemberDTO>> GetAllMember()
        {
            using var connection = _context.CreateConnection();


            // using Dapper's multi-mapping
            var dtoSql = @"
SELECT m.*,g.*
			FROM Members AS m
            JOIN `Groups` AS g ON m.Group_id = g.Group_id ;";

            var users = await connection.QueryAsync<MemberDTO, GroupsDTOs, MemberDTO>(dtoSql,
                (member, group) =>
                {
                    member.groups = group;// Assuming 'AccountDTO' has a property 'Member' of type 'Member'
                    return member;           // Return the account with its member populated
                },
                splitOn: "Group_id");
            return users.ToList();
        }

        public async Task<MemberDTO> GetMember(string name)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
    SELECT m.*, g.*
    FROM Members AS m
    JOIN `Groups` AS g ON m.Group_id = g.Group_id 
    WHERE m.name = @name";
            var member = await connection.QueryAsync<MemberDTO, GroupsDTOs, MemberDTO>(
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
            var member = await connection.QueryAsync<MemberDTO, GroupsDTOs, MemberDTO>(
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
    }
}
