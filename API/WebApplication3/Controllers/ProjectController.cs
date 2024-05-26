using K4os.Compression.LZ4.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using WebApplication3.DTOs.Project;
using WebApplication3.DTOs.Sponsor;
using WebApplication3.Entities;
using WebApplication3.repository.AccountRepository;
using WebApplication3.Service.AccountService;
using WebApplication3.Service.MemberService;
using WebApplication3.Service.ProjectService;
using WebApplication3.Service.SponsorService;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService IProjectService;
        public ProjectController(IProjectService IProjectService)
        {
            this.IProjectService = IProjectService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProject([FromQuery] int pageNumber = 1)
        {
            var result = await IProjectService.GetAllProject(pageNumber);
            return Ok(new { projects = result.Projects, totalPages = result.TotalPages });
        }
        [HttpGet("{name}")]
        public async Task<ActionResult<Project>> GetProject(string name)
        {
            var pro = await IProjectService.GetProjectsByName(name);
            return Ok(pro);
        }
        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            await IProjectService.DeleteProject(name);
            return Ok(new { message = "Project deleted" });
        }
        [HttpPost]
        public async Task<IActionResult> AddProject(CreateProjectRequest createProject)
        {
            try
            {
                await IProjectService.AddProject(createProject);
                return Ok($"Project '{createProject.Name}' Created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error Created project: {ex.Message}");
            };
        }
        [HttpPut]
        public async Task<ActionResult> UpdateProject(string name, UpdateProjectRequest up)
        {
            try
            {
                await IProjectService.UpdateProject(name, up);
                return Ok($"Project '{name}' updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating project: {ex.Message}");
            };
        }

    }
}
