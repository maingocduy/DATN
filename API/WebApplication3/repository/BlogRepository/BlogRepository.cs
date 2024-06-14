using Dapper;
using Microsoft.EntityFrameworkCore;
using WebApplication3.DTOs.Account;
using WebApplication3.DTOs.Blog;
using WebApplication3.DTOs.Groups;
using WebApplication3.DTOs.Member;
using WebApplication3.DTOs.Project;
using WebApplication3.DTOs.Sponsor;
using WebApplication3.Entities;
using WebApplication3.Helper.Data;
using static System.Net.WebRequestMethods;

namespace WebApplication3.repository.BlogRepository
{
    public interface IBlogRepository
    {
        Task<List<BlogDTO>> GetAllBlogs();
        Task<BlogDTO> GetBlog(int id);
        Task<BlogDTO> GetBlogsByTitle(string title);
        Task UpdateBlog(BlogDTO blog);
        Task DeleteBlog(BlogDTO blog);
        Task<List<BlogDTO>> GetAllBlogsTrue();
        Task UpdateStatus(bool Approved, int id);
        Task AddBlog(int id, BlogDTO blog);
    }
    public class BlogRepository : IBlogRepository
    {
        private AppDbContext _context;
        public BlogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddBlog(int id, BlogDTO blog)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
    INSERT INTO Blog (Account_id, Title, Content)
    VALUES (@acc_id, @title, @content)";

            await connection.ExecuteAsync(sql, new
            {
                title = blog.Title,
                content = blog.Content,
                acc_id = id
            });
        }



        public Task DeleteBlog(BlogDTO blog)
        {
            throw new NotImplementedException();
        }
        public async Task UpdateStatus(bool Approved, int id)
        {
            using var connection = _context.CreateConnection();
            var sql = "UPDATE Blog SET Approved = @Approved Where Blog_id = @id";
            await connection.ExecuteAsync(sql, new
            {
                Approved = Approved,
                id = id
            });
        }
        public async Task<List<BlogDTO>> GetAllBlogs()
        {
            using var connection = _context.CreateConnection();
            var sql = @"
    SELECT b.*, a.*
    FROM Blog AS b
    LEFT JOIN account AS a ON b.Account_id = a.Account_id";
            var blog = await connection.QueryAsync<BlogDTO, AccountDTO,BlogDTO>(
        sql,
        (blog, account) =>
        {
            blog.account = account;
            return blog; 
        },
        splitOn: "Account_id"
    );
            return blog.ToList();
        }

        public async Task<List<BlogDTO>> GetAllBlogsTrue()
        {
            using var connection = _context.CreateConnection();
            var sql = @"
    SELECT b.*, a.*
    FROM Blog AS b
    LEFT JOIN account AS a ON b.Account_id = a.Account_id
    WHERE Approved = true";
            var blog = await connection.QueryAsync<BlogDTO, AccountDTO, BlogDTO>(
        sql,
        (blog, account) =>
        {
            blog.account = account;
            return blog;
        },
        splitOn: "Account_id"
    );
            return blog.ToList();
        }
        public async Task<BlogDTO> GetBlog(int id)
        {

            using var connection = _context.CreateConnection();
            var sql = """
        SELECT b.*,a.*
        FROM Blog AS b
        LEFT JOIN account AS a ON b.Account_id = a.Account_id
        WHERE Blog_id = @id;
        """;
            var blog = await connection.QueryAsync<BlogDTO,AccountDTO, BlogDTO>(
        sql,
        (blog, account) =>
        {
            blog.account = account;
           
            return blog;
        },new { id },
        splitOn: "Account_id"
    );
            return blog.FirstOrDefault();
        }


    public async Task<BlogDTO> GetBlogsByTitle(string title)
        {
            using var connection = _context.CreateConnection();
            var sql = """
        SELECT b.*, a.*
        FROM Blog AS b
         LEFT JOIN account AS a ON b.Account_id = a.Account_id
        WHERE Blog_id = @id;
        """;
            var blog = await connection.QueryAsync<BlogDTO, AccountDTO, BlogDTO>(
        sql,
        (blog, account) =>
        {
            blog.account = account;

            return blog;
        }, new { title },
        splitOn: "Account_id"
    );
            return blog.FirstOrDefault();
        }

        public Task UpdateBlog(BlogDTO blog)
        {
            throw new NotImplementedException();
        }
    }
}
