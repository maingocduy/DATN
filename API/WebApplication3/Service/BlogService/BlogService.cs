using AutoMapper;
using K4os.Compression.LZ4.Internal;
using Org.BouncyCastle.Asn1.Ocsp;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Xml.Linq;
using WebApplication3.DTOs.Account;
using WebApplication3.DTOs.Blog;
using WebApplication3.DTOs.Member;
using WebApplication3.repository.AccountRepository;
using WebApplication3.repository.BlogRepository;
using WebApplication3.repository.GroupsRepository;
using WebApplication3.repository.MemberRepository;
using WebApplication3.repository.ProjectReposiotry;

namespace WebApplication3.Service.BlogService
{
    public interface IBlogService
    {
        Task<List<BlogDTO>> GetAllBlog();
        Task<AccountDTO> GetBlogsAsync(int id);

        Task<AccountDTO> GetBlogsByTitle(string title);

        Task UpdateBlog(string title, UpdatePasswordRequestDTO acc);
        Task DeleteBlog(string title);
        Task UpdateStatus(updateApprovedRequest request);
        Task AddBlog(string jwt, CreateRequestBLogDTO create);
        Task<List<BlogDTO>> GetAllBlogTrue();
    }
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository IBlogRepository;
        private readonly IMapper _mapper;
        private readonly IAccountRepository IAccountRepository;
        public BlogService(IBlogRepository IBlogRepository, IAccountRepository IAccountRepository, IMapper mapper)
        {
            this.IBlogRepository = IBlogRepository;
            this.IAccountRepository = IAccountRepository;
            _mapper = mapper;
        }

        public async Task AddBlog(string jwt, CreateRequestBLogDTO create)
        {

            // Giải mã JWT và trích xuất thông tin người dùng
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(jwt);
            // Lấy các claim từ JWT để lấy thông tin người dùng
            var userName = jwtToken.Claims.Where(
                c => c.Type == ClaimTypes.Name
                ).FirstOrDefault().Value;
            var BlogDTO = _mapper.Map<BlogDTO>(create);

            var id = await IAccountRepository.getIDAcount(userName);
            await IBlogRepository.AddBlog(id, BlogDTO);
      
        }

        public async Task UpdateStatus(updateApprovedRequest request)
        {
            try
            {
                var getBlog = await IBlogRepository.GetBlog(request.Id);
                if (getBlog == null)
                {
                    throw new KeyNotFoundException("Không tìm thấy blog");
                }
                if (getBlog.Approved)
                {
                    await IBlogRepository.UpdateStatus(false, request.Id);
                }
                else
                {
                    await IBlogRepository.UpdateStatus(true, request.Id);
                }
            }
            catch(KeyNotFoundException ex) {
                throw ex;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public Task DeleteBlog(string title)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BlogDTO>> GetAllBlog()
        {
            return await IBlogRepository.GetAllBlogs();
        }
        public async Task<List<BlogDTO>> GetAllBlogTrue()
        {
            return await IBlogRepository.GetAllBlogsTrue();
        }
        public Task<AccountDTO> GetBlogsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AccountDTO> GetBlogsByTitle(string title)
        {
            throw new NotImplementedException();
        }

        public Task UpdateBlog(string title, UpdatePasswordRequestDTO acc)
        {
            throw new NotImplementedException();
        }
    }
}
