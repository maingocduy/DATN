using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.DTOs.Blog;
using WebApplication3.DTOs.Project;
using WebApplication3.Entities;
using WebApplication3.Service.AccountService;
using WebApplication3.Service.BlogService;
using WebApplication3.Service.ProjectService;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService IBlogService;
        public BlogController(IBlogService IBlogService)
        {
            this.IBlogService = IBlogService;
        }
        [HttpGet]
        public async Task<ActionResult<List<blog>>> GetAllBlog()
        {
            var blog = await IBlogService.GetAllBlog();
            return Ok(blog);
        }
        [HttpPost, Authorize]
        public async Task<IActionResult> AddBlog(CreateRequestBLogDTO createBlog)
        {
            try
            {
                var jwt = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
                await IBlogService.AddBlog(jwt,createBlog);
                return Ok($"Project '{createBlog.Title}' Created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error Created project: {ex.Message}");
            };
        }
    }
}
