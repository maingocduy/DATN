using Dapper;
using Microsoft.EntityFrameworkCore;
using WebApplication3.DTOs.Account;
using WebApplication3.DTOs.Blog;
using WebApplication3.DTOs.Groups;
using WebApplication3.DTOs.Member;
using WebApplication3.DTOs.Project;
using WebApplication3.DTOs.Sponsor;
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
        INSERT INTO Blog (Member_id, Title, Content, CreatedAt)
        VALUES (@member_id, @title, @content, @createdAt)";

            await connection.ExecuteAsync(sql, new
            {
                title = blog.Title,
                content = blog.Content,
                member_id = id,
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
        SELECT b.*, m.*, g.*
        FROM Blog AS b
        LEFT JOIN Members AS m ON b.Member_id = m.Member_id
        LEFT JOIN `Groups` AS g ON m.Group_id = g.Group_id;
        """;
            var blog = await connection.QueryAsync<BlogDTO, MemberDTO, GroupsDTOs,BlogDTO>(
        sql,
        (blog, member, group) =>
        {
            blog.Member = member;
            member.groups = group;
            return blog; 
        },
        splitOn: "Member_id,Group_id"
    );
            return blog.ToList();
        }

        public async Task<BlogDTO> GetBlog(int id)
        {

            using var connection = _context.CreateConnection();
            var sql = """
        SELECT b.*, m.*, g.*,p.*
        FROM Blog AS b
        LEFT JOIN Members AS m ON b.Member_id = m.Member_id
        LEFT JOIN `Groups` AS g ON m.Group_id = g.Group_id
        LEFT JOIN Projects AS p ON p.Project_id = b.Project_id where Blog_id = @id;
        """;
            var blog = await connection.QueryAsync<BlogDTO, MemberDTO, GroupsDTOs, ProjectDTO, BlogDTO>(
        sql,
        (blog, member, group, project) =>
        {
            blog.Member = member;
            member.groups = group;
            blog.project = project;
            return blog;
        },new { id },
        splitOn: "Member_id,Group_id,Project_id"
    );
            return blog.FirstOrDefault();
        }


    public async Task<BlogDTO> GetBlogsByTitle(string title)
        {
            using var connection = _context.CreateConnection();
            var sql = """
        SELECT b.*, m.*, g.*,p.*
        FROM Blog AS b
        LEFT JOIN Members AS m ON b.Member_id = m.Member_id
        LEFT JOIN `Groups` AS g ON m.Group_id = g.Group_id
        LEFT JOIN Projects AS p ON p.Project_id = b.Project_id where Title = @title;
        """;
            var blog = await connection.QueryAsync<BlogDTO, MemberDTO, GroupsDTOs, ProjectDTO, BlogDTO>(
        sql,
        (blog, member, group, project) =>
        {
            blog.Member = member;
            member.groups = group;
            blog.project = project;
            return blog;
        }, new { title },
        splitOn: "Member_id,Group_id,Project_id"
    );
            return blog.FirstOrDefault();
        }

        public Task UpdateBlog(BlogDTO blog)
        {
            throw new NotImplementedException();
        }
    }
}
