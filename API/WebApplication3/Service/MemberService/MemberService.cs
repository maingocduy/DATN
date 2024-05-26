using AutoMapper;
using CloudinaryDotNet.Core;
using System.Xml.Linq;
using WebApplication3.DTOs.Member;
using WebApplication3.DTOs.Sponsor;
using WebApplication3.repository.GroupsRepository;
using WebApplication3.repository.MemberRepository;
using WebApplication3.repository.ProjectReposiotry;
using WebApplication3.repository.SponsorRepository;

namespace WebApplication3.Service.MemberService
{
    public interface IMemberService
    {
        Task<List<MemberDTO>> GetAllMember();
        Task<MemberDTO> GetMemberAsync(int id);

        Task<MemberDTO> GetMember(string member);
        Task AddMember(string name, CreateRequestMemberDTO acc);
        Task UpdateMember(UpdateRequestMember update);
        Task DeleteMember(string member);

        Task JoinProject(string project_name, string member_name);
    }
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository IMemberRepository;
        private readonly IMapper _mapper;
        private readonly IProjectRepository projectRepository;
        public MemberService(IMemberRepository IMemberRepository,IProjectRepository projectRepository, IMapper mapper)
        {
            this.IMemberRepository = IMemberRepository;
            _mapper = mapper;
            this.projectRepository = projectRepository;

        }
        public async Task AddMember(string name, CreateRequestMemberDTO mem)
        {
            // Kiểm tra xem dự án có tồn tại không
            var project = await projectRepository.GetProjectID(name);
            if (project == null)
            {
                throw new KeyNotFoundException("Project not found");
            }
            if(await IMemberRepository.GetMember(mem.Name) == null)
            {
                var memberDTO = _mapper.Map<MemberDTO>(mem);

                await IMemberRepository.AddMember(project.Project_id, memberDTO);
            }
            else throw new Exception($"Member with the email '{mem.Email}' already exist");
        }

        public async Task DeleteMember(string name)
        {
            await IMemberRepository.DeleteMember(name);
        }

        public async Task<List<MemberDTO>> GetAllMember()
        {
            return await IMemberRepository.GetAllMember();
        }

        public async Task<MemberDTO> GetMember(string member)
        {
            var group = await IMemberRepository.GetMember(member);

            if (group == null)
                throw new KeyNotFoundException("member not found");

            return group;
        }

        public Task<MemberDTO> GetMemberAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task JoinProject(string project_name, string member_name)
        {
            var member = await IMemberRepository.GetMember(member_name);

            var project = await projectRepository.GetProject(project_name);

            if (member== null || project == null)
            {
                throw new Exception($"Member or project does not exist");
            }

            await IMemberRepository.JoinProject(project.Project_id, member.Member_id);
        }

        public async Task UpdateMember(UpdateRequestMember update)
        {
            // Kiểm tra xem thành viên có tồn tại không
            var existingMember = await IMemberRepository.GetMember(update.Name);
            if (existingMember == null)
            {
                throw new Exception($"Member with the name '{update.Name}' does not exist");
            }

            // Kiểm tra xem email mới đã tồn tại cho một thành viên khác chưa
            var existingMemberByEmail = await IMemberRepository.GetMemberByEmail(update.Email);
            if (existingMemberByEmail != null && existingMemberByEmail.email != existingMember.email)
            {
                throw new Exception($"Member with the email '{update.Email}' already exists for another member");
            }

            // Cập nhật thông tin thành viên
            existingMember.name = update.Name;
            existingMember.phone = update.Phone;
            existingMember.email = update.Email;

            // Gọi repository để cập nhật thông tin thành viên
            await IMemberRepository.UpdateMember(existingMember);
        }


    }
}
