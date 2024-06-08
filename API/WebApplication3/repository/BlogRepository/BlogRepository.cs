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

namespace WebApplication3.repository.BlogRepository
{
    public interface IBlogRepository
    {
        Task<List<BlogDTO>> GetAllBlogs();
        Task<BlogDTO> GetBlog(int id);
        Task<BlogDTO> GetBlogsByTitle(string title);
        Task UpdateBlog(BlogDTO blog);
        Task DeleteBlog(BlogDTO blog);

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
        INSERT INTO Blog (Account_id, Title, Content, CreatedAt)
        VALUES (@acc_id, @title, @content, @createdAt)";

            await connection.ExecuteAsync(sql, new
            {
                title = blog.Title,
                content = blog.Content,
                acc_id = id,
                createdAt = DateTime.Now
            });
        }


        public Task DeleteBlog(BlogDTO blog)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BlogDTO>> GetAllBlogs()
        {
            using var connection = _context.CreateConnection();
            var sql = """
        SELECT b.*, a.*
        FROM account 
        FROM Blog AS b
        LEFT JOIN account AS a ON b.Account_id = a.Account_id;
        """;
            var blog = await connection.QueryAsync<BlogDTO, AccountDTO, GroupsDTOs,BlogDTO>(
        sql,
        (blog, account, group) =>
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
