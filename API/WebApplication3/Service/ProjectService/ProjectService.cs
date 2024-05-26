using AutoMapper;
using K4os.Compression.LZ4.Internal;
using System.ComponentModel.DataAnnotations;
using WebApplication3.DTOs;
using WebApplication3.DTOs.Account;
using WebApplication3.DTOs.Project;
using WebApplication3.repository.AccountRepository;
using WebApplication3.repository.CloudRepository;
using WebApplication3.repository.MemberRepository;
using WebApplication3.repository.ProjectReposiotry;
using WebApplication3.Service.Cloudinary_image;

namespace WebApplication3.Service.ProjectService
{
    public interface IProjectService
    {
        Task<PagedResult<ProjectDTO>> GetAllProject(int pageNumber);
        Task<ProjectDTO> GetProject(int id);
        Task AddProject(CreateProjectRequest createProjectRequest);
        Task<ProjectDTO> GetProjectsByName(string name);

        Task UpdateProject(string name, UpdateProjectRequest project);
        Task DeleteProject(string name);


        
    }
    public class ProjectService(IProjectRepository IProjectRepository,ICloudinaryService cloudinaryService, IMapper _mapper, ILogger<ProjectService> logger) : IProjectService
    {
        public async Task DeleteProject(string name)
        {
            await cloudinaryService.GetPublicIdByProjectName(name);
            await IProjectRepository.DeleteProject(name);
        }

        public async Task<PagedResult<ProjectDTO>> GetAllProject(int pageNumber)
        {

            // Sử dụng IProjectRepository để lấy dự án từ cơ sở dữ liệu
            var projects = await IProjectRepository.GetAllProject(pageNumber);

            // Trả về kết quả dưới dạng tranginated (phân trang)
            return projects;
        }

        public Task<ProjectDTO> GetProject(int id)
        {
            throw new NotImplementedException();
        }

        public async Task AddProject(CreateProjectRequest createRequest)
        {
            try
            {
                if(await IProjectRepository.GetProject(createRequest.Name) != null)
                {
                    throw new Exception("Project with the name '" + createRequest.Name + "' already exists");
                }
                ValidateRequest(createRequest);

                // Convert CreateProjectRequest to ProjectDTO
                var projectDto = new ProjectDTO
                {
                    Name = createRequest.Name,
                    Budget = createRequest.Budget,
                    Description = createRequest.Description,
                    StartDate = (DateTime)createRequest.StartDate,
                    EndDate = (DateTime)createRequest.EndDate,
                    // Assume other necessary fields are set accordingly
                };

                // Call repository to add the project
                await IProjectRepository.AddProject(projectDto);

                logger.LogInformation("Project added successfully: {ProjectName}", createRequest.Name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding project: {ProjectName}", createRequest.Name);
                throw;
            }

        }

        public async Task<ProjectDTO> GetProjectsByName(string name)
        {
            var project = await IProjectRepository.GetProject(name);

            if (project == null)
                throw new KeyNotFoundException("Project not found");

            return project;
        }

        public async Task UpdateProject(string name, UpdateProjectRequest updateRequest)
        {
            try
            {
                // Kiểm tra xem dự án có tồn tại không
                var existingProject = await IProjectRepository.GetProject(name);
                if (existingProject == null)
                {
                    throw new Exception($"Project with the name '{name}' does not exist");
                }

                // Kiểm tra xem dự án có bị trùng tên không

                // Validate update request
                ValidateUpdateRequest(updateRequest);

                // Cập nhật dữ liệu dự án
                existingProject.Budget = updateRequest.Budget;
                existingProject.Description = updateRequest.Description;
                existingProject.StartDate = updateRequest.StartDate;
                existingProject.EndDate = updateRequest.EndDate;
                // Assume other necessary fields are set accordingly

                // Gọi repository để cập nhật dự án
                await IProjectRepository.UpdateProject(existingProject);

                logger.LogInformation("Project updated successfully: {ProjectName}", existingProject.Name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating project: {ProjectName}", name);
                throw;
            }
        }
        private void ValidateRequest(CreateProjectRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var context = new ValidationContext(request, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(request, context, validationResults, true))
            {
                string errorMessage = string.Join(Environment.NewLine, validationResults.Select(vr => vr.ErrorMessage));
                throw new ValidationException(errorMessage);
            }

            // Add additional custom validations if needed
            if (request.EndDate <= request.StartDate)
            {
                throw new ArgumentException("End date must be after start date.");
            }
        }
        private void ValidateUpdateRequest(UpdateProjectRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var context = new ValidationContext(request, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(request, context, validationResults, true))
            {
                string errorMessage = string.Join(Environment.NewLine, validationResults.Select(vr => vr.ErrorMessage));
                throw new ValidationException(errorMessage);
            }

            // Add additional custom validations if needed
            if (request.EndDate <= request.StartDate)
            {
                throw new ArgumentException("End date must be after start date.");
            }
        }
        

    }
    }
