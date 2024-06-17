using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.DTOs.Blog;
using WebApplication3.DTOs.Project;
using WebApplication3.Entities;
using WebApplication3.Helper;
using WebApplication3.Helper.Data;
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
        [HttpGet("all_blog"), Authorize]
        public async Task<ActionResult<List<blog>>> GetAllBlog()
        {
            var blog = await IBlogService.GetAllBlog();
            return Ok(blog);
        }
        [HttpGet("all_blog_approve")]
        public async Task<ActionResult<List<blog>>> GetAllBlogApprove()
        {
            var blog = await IBlogService.GetAllBlogTrue();
            return Ok(blog);
        }
        [HttpPost, Authorize]
        public async Task<IActionResult> AddBlog(CreateRequestBLogDTO createBlog)
        {
            try
            {
                var jwt = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
                await IBlogService.AddBlog(jwt,createBlog);
                return Ok(new { Message = "Thêm thành công blog mới!" });
            }
            catch (System.UnauthorizedAccessException ex)
            {
                // Xử lý và trả về lỗi 401
                return StatusCode(401, $"Unauthorized access: {ex.Message}");
            }
            catch (ForbiddenAccessException ex)
            {
                // Xử lý và trả về lỗi 403
                return StatusCode(403, $"Forbidden access: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Xử lý các ngoại lệ khác và trả về lỗi 500
                return BadRequest(new { Message = ex.Message });
            };
          
        }

        [HttpPost("update_status"), Authorize]
        public async Task<IActionResult> UpdateStatus(updateApprovedRequest request)
        {
            try
            {
                await IBlogService.UpdateStatus(request);
                return Ok(new { Message = "Duyệt thành công" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
