using Dapper;
using Microsoft.EntityFrameworkCore;
using WebApplication3.DTOs;
using WebApplication3.DTOs.Account;
using WebApplication3.DTOs.Groups;
using WebApplication3.DTOs.ImageDto;
using WebApplication3.DTOs.Member;
using WebApplication3.DTOs.Project;
using WebApplication3.DTOs.Sponsor;
using WebApplication3.Entities;
using WebApplication3.Helper.Data;
using static System.Net.Mime.MediaTypeNames;

namespace WebApplication3.repository.ProjectReposiotry
{
    public interface IProjectRepository
    {
        Task AddProject(ProjectDTO project);
        Task DeleteProject(string name);
        Task<ProjectDTO> GetProject(string name, int? groupId = null);
        Task UpdateProject(ProjectDTO project);
        Task<PagedResult<ProjectDTO>> GetAllProject(int pageNumber = 1);
        Task<ProjectDTO> GetProjectID(string name);
    }
    public class ProjectRepository : IProjectRepository
    {
        private AppDbContext _context;
        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddProject(ProjectDTO project)
        {
            using var connection = _context.CreateConnection();

            // Câu lệnh SQL để chèn một dự án mới
            var insertSql = @"
INSERT INTO Projects (Name, Description, StartDate, EndDate, Budget)
VALUES (@Name, @Description, @StartDate, @EndDate, @Budget);
";

            // Thực hiện chèn dữ liệu dự án mới
            await connection.ExecuteAsync(insertSql, new
            {
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Budget = project.Budget
            });
        }


        public async Task DeleteProject(string name)
        {
            using var connection = _context.CreateConnection();

            // Đầu tiên, bạn cần xóa các mục phụ thuộc có thể tồn tại như chi tiết của dự án
            // ví dụ: xóa các mục trong MemberProjects mà có liên quan đến dự án
            var deleteMemberProjectsSql = @"
DELETE FROM MemberProjects
WHERE Project_id IN (
    SELECT Project_id FROM Projects WHERE Name = @ProjectName
);";

            // Thực hiện câu lệnh SQL xóa MemberProjects
            await connection.ExecuteAsync(deleteMemberProjectsSql, new { ProjectName = name });
            var deleteProject_ImagesSql = @"
DELETE FROM Project_image
WHERE Project_id IN (
    SELECT Project_id FROM Projects WHERE Name = @ProjectName
);";

            // Thực hiện câu lệnh SQL xóa MemberProjects
            await connection.ExecuteAsync(deleteMemberProjectsSql, new { ProjectName = name });
            // Sau đó, xóa dự án
            var deleteProjectSql = @"
DELETE FROM Projects
WHERE Name = @ProjectName;";

            // Thực hiện câu lệnh SQL xóa Project
            await connection.ExecuteAsync(deleteProjectSql, new { ProjectName = name });
        }

        public async Task<PagedResult<ProjectDTO>> GetAllProject(int pageNumber = 1)
        {
            using var connection = _context.CreateConnection();
            int pageSize = 6;
            // Tính toán offset
            var offset = (pageNumber - 1) * pageSize;

            // Câu lệnh SQL mới
            var dtoSql = @"
SELECT p.*, m.*, g.*, s.*, i.*
FROM Projects AS p
LEFT JOIN MemberProjects AS mp ON p.Project_id = mp.Project_id
LEFT JOIN Members AS m ON mp.Member_id = m.Member_id
LEFT JOIN `Groups` AS g ON m.Group_id = g.Group_id
LEFT JOIN ProjectSponsor AS ps ON p.Project_id = ps.Project_id
LEFT JOIN sponsor AS s ON ps.Sponsor_id = s.Sponsor_id
LEFT JOIN Project_image AS i ON i.Project_id = p.Project_id
LIMIT @pageSize OFFSET @offset;
";

            // Thực hiện truy vấn
            var projectsQuery = await connection.QueryAsync<ProjectDTO, MemberDTO, Group, SponsorDTO, ImageDtos, ProjectDTO>(
                dtoSql,
                (project, member, group, sponsor, image) =>
                {
                    project.Member ??= new List<MemberDTO>();
                    project.Sponsor ??= new List<SponsorDTO>();
                    project.images ??= new List<ImageDtos>();

                    if (member != null)
                    {
                        project.Member.Add(member);
                        member.groups = group;
                    }
                    if (sponsor != null)
                    {
                        project.Sponsor.Add(sponsor);
                    }
                    if (image != null)
                    {
                        project.images.Add(image);
                    }
                    return project;
                },
                new { pageSize, offset },
                splitOn: "Member_id,Group_id,Sponsor_id,image_id");

            var projects = projectsQuery.GroupBy(p => p.Project_id).Select(group =>
            {
                var groupedProject = group.First();

                if (groupedProject.Member != null && groupedProject.Member.Any())
                {
                    groupedProject.Member = group.Select(p => p.Member.FirstOrDefault()).ToList();
                }
                if (groupedProject.Sponsor != null && groupedProject.Sponsor.Any())
                {
                    groupedProject.Sponsor = group.Select(p => p.Sponsor.FirstOrDefault()).ToList();
                }
                if (groupedProject.images != null && groupedProject.images.Any())
                {
                    groupedProject.images = group.Select(p => p.images.FirstOrDefault()).ToList();
                }
                return groupedProject;
            }).ToList();

            // Lấy tổng số dự án
            var countSql = "SELECT COUNT(*) FROM Projects";
            var totalCount = await connection.ExecuteScalarAsync<int>(countSql);

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PagedResult<ProjectDTO>
            {
                Projects = projects,
                TotalPages = totalPages
            };
        }


        public async Task<ProjectDTO> GetProject(string name, int? groupId = null)
        {
            using var connection = _context.CreateConnection();

            // Câu lệnh SQL để lấy dự án theo tên và lọc theo Group_id nếu có
            var dtoSql = @"
SELECT 
    p.*,
    m.* ,
    g.* ,
    s.* ,
    i.* 
FROM 
    Projects AS p
    LEFT JOIN MemberProjects AS mp ON p.Project_id = mp.Project_id
    LEFT JOIN Members AS m ON mp.Member_id = m.Member_id
    LEFT JOIN `Groups` AS g ON m.Group_id = g.Group_id
    LEFT JOIN ProjectSponsor AS ps ON p.Project_id = ps.Project_id
    LEFT JOIN sponsor AS s ON ps.Sponsor_id = s.Sponsor_id
    LEFT JOIN Project_image AS i ON i.Project_id = p.Project_id
WHERE 
    p.Name = @ProjectName";

            // Nếu có GroupId, thêm điều kiện lọc cho Members
            if (groupId.HasValue)
            {
                dtoSql += " AND m.Member_id IN (SELECT Member_id FROM Members WHERE Group_id = @GroupId)";
            }

            // Thực hiện truy vấn
            var projectQuery = await connection.QueryAsync<ProjectDTO, MemberDTO, Group, SponsorDTO, ImageDtos, ProjectDTO>(
                dtoSql,
                (project, member, group, sponsor, image) =>
                {
                    // Kiểm tra và khởi tạo danh sách thành viên, nhóm, nhà tài trợ và ảnh nếu cần
                    project.Member ??= new List<MemberDTO>();
                    project.Sponsor ??= new List<SponsorDTO>();
                    project.images ??= new List<ImageDtos>();

                    // Thêm thành viên, nhóm, nhà tài trợ và ảnh nếu không null
                    if (member != null)
                    {
                        project.Member.Add(member);
                        member.groups = group; // Gán nhóm cho thành viên
                    }
                    if (sponsor != null)
                    {
                        project.Sponsor.Add(sponsor);
                    }
                    if (image != null)
                    {
                        project.images.Add(image);
                    }

                    return project;
                },
                new { ProjectName = name, GroupId = groupId }, // Đối tượng ẩn danh với tên dự án và GroupId nếu có
                splitOn: "Member_id,Group_id,Sponsor_id,image_id");

            // Lấy dự án đầu tiên hoặc trả về null nếu không có kết quả
            var project = projectQuery.FirstOrDefault();

            // Nếu không có thành viên được trả về, gán danh sách thành viên là rỗng
            if (project != null && project.Member.Count == 0)
            {
                project.Member = new List<MemberDTO>();
            }

            return project;
        }


        public async Task<ProjectDTO> GetProjectID(string name)
        {
            using var connection = _context.CreateConnection();

            // Câu lệnh SQL truy vấn ID của dự án
            var sql = @"
        SELECT Project_id FROM Projects
        WHERE Name = @name;
    ";

            // Thực hiện truy vấn và trả về ID của dự án
            return await connection.QueryFirstOrDefaultAsync<ProjectDTO>(sql, new { name });
        }

        public async Task UpdateProject(ProjectDTO project)
        {
            using var connection = _context.CreateConnection();

            // Câu lệnh SQL cập nhật dự án
            var sql = @"
                UPDATE Projects
                SET Name = @Name, Budget = @Budget, Description = @Description, 
                    StartDate = @StartDate, EndDate = @EndDate
                WHERE Project_id = @Project_id;";

            // Thực hiện cập nhật
            await connection.ExecuteAsync(sql, project);

        }
    }
}
