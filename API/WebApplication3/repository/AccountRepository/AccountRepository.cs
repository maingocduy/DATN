using Dapper;
using Microsoft.EntityFrameworkCore;
using WebApplication3.DTOs.Account;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using WebApplication3.Entities;
using WebApplication3.DTOs.Groups;
using WebApplication3.DTOs.Member;
using WebApplication3.Helper.Data;
using WebApplication3.DTOs.Otp;
using Org.BouncyCastle.Crypto;

namespace WebApplication3.repository.AccountRepository
{
    public interface IAccountRepository
    {
        Task<List<AccountDTO>> GetAllAcc();
        Task<AccountDTO> GetAccounts(int id);
        Task<AccountDTO> GetAccountsByUserName(string username);
        Task UpdatePasswordAcc(AccountDTO acc);
        Task DeleteAccount(AccountDTO acc);
        Task<int> getIDAcount(string name);
        Task<OtpDTO> GetOtp(string otp);

        Task UpdateOtp(OtpDTO otp);

        Task SaveOtp(string otp, string emailAccount);
    }
    public class AccountRepository : IAccountRepository
    {
        private AppDbContext _context;
        public AccountRepository(AppDbContext context) {
            _context = context;
        }

        public async Task DeleteAccount(AccountDTO acc)
        {
            using var connection = _context.CreateConnection();

            // Xóa tài khoản từ bảng account
            var sqlAccount = @"
        DELETE FROM account 
        WHERE Username = @username
    ";
            await connection.ExecuteAsync(sqlAccount, new { username = acc.Username });

            // Xóa thành viên từ bảng Members
            var sqlMember = @"
        DELETE FROM Members 
        WHERE Member_id = @member_id
    ";
            await connection.ExecuteAsync(sqlMember, new { member_id = acc.Member.Member_id });
        }


        public async Task<AccountDTO> GetAccounts(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = """
            SELECT a.*, m.*,g.*
        			FROM account AS a
        			JOIN Members AS m ON a.Member_id = m.Member_id
                    JOIN `Groups` AS g ON m.Group_id = g.Group_id 
            WHERE Account_id = @id
        """;
            var acc = await connection.QueryAsync<AccountDTO, MemberDTO, GroupsDTOs, AccountDTO>(
        sql,
        (account, member, group) =>
        {
            account.Member = member;
            member.groups = group; // Assuming 'MemberDTO' has a property 'groups' of type 'GroupsDTOs'
            return account; // Return the account with its member populated
        },
        new { id },
        splitOn: "Member_id,Group_id"
    );
            return acc.FirstOrDefault();
        }
      
        public async Task<AccountDTO> GetAccountsByUserName(string username)
        {
            using var connection = _context.CreateConnection();
            var sql = """
            SELECT a.*, m.*,g.*
        			FROM account AS a
        			JOIN Members AS m ON a.Member_id = m.Member_id
                    JOIN `Groups` AS g ON m.Group_id = g.Group_id 
            WHERE Username = @username
        """;
            var acc= await connection.QueryAsync<AccountDTO, MemberDTO, GroupsDTOs, AccountDTO>(
       sql,
       (account, member, group) =>
       {
           account.Member = member;
           member.groups = group; // Assuming 'MemberDTO' has a property 'groups' of type 'GroupsDTOs'
           return account; // Return the account with its member populated
       },
       new { username },
       splitOn: "Member_id,Group_id"
   );
            return acc.FirstOrDefault();
        }

        public async Task<List<AccountDTO>> GetAllAcc()
        {
            using var connection = _context.CreateConnection();


            // using Dapper's multi-mapping
            var dtoSql = @"
SELECT a.*, m.*,g.*
			FROM account AS a
			JOIN Members AS m ON a.Member_id = m.Member_id
            JOIN `Groups` AS g ON m.Group_id = g.Group_id ;";

            var users = await connection.QueryAsync<AccountDTO, MemberDTO,GroupsDTOs, AccountDTO>(dtoSql,
                (account, member,group) =>
                {
                    account.Member = member;
                    member.groups = group;// Assuming 'AccountDTO' has a property 'Member' of type 'Member'
                    return account;           // Return the account with its member populated
                },
                splitOn: "Member_id,Group_id");
            return users.ToList();
        }

        public async Task UpdatePasswordAcc(AccountDTO acc)
        {
            using var connection = _context.CreateConnection();
            var sql = """
            UPDATE account 
            SET Password = @Password
            WHERE Username = @Username
        """;

            await connection.ExecuteAsync(sql, acc);
        }
        public async Task<int> getIDAcount(string name)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
            SELECT a.Member_id
            FROM Account AS a
            JOIN Members AS m ON m.Member_id= a.Member_id
            WHERE Username = @name";

            var memberId = await connection.QueryFirstOrDefaultAsync<int>(sql, new { name });

            return memberId;
        }
        public async Task SaveOtp(string otp,string emailAccount)
        {
            using var connection = _context.CreateConnection();
            var sql = """
            INSERT INTO otp_table (otp_code, created_at,expires_at,Account_id)
            VALUES (@otp, @created_at,@expires_at,(Select Account_id from Account where Member_id = (Select Member_id from Members where email = @email)))
        """;
            var vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // Mã này tương ứng với GMT+7
            var vnTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTimeZone);
            await connection.ExecuteAsync(sql, new
            {
                otp = otp,
                created_at = vnTime,
                expires_at = vnTime.AddMinutes(3),
                email = emailAccount
            });
        }
        public async Task<OtpDTO> GetOtp(string otp)
        {
            using var connection = _context.CreateConnection();
            var sql = """
    SELECT *
           FROM otp_table
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
        			FROM otp_table
        			
            WHERE otp_code = @otp
        """;
            await connection.QueryAsync(sql, new { otp });
            
        }
        public async Task UpdateOtp(OtpDTO otp)
        {
            using var connection = _context.CreateConnection();
            var sql = """
        UPDATE otp_table
        SET IsVerified = @IsVerified
        WHERE otp_code = @OtpCode
        """;
            await connection.ExecuteAsync(sql, new
            {
                IsVerified = otp.IsVerified,
                OtpCode = otp.otp_code
            });
        }
    }
}
