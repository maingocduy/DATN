using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebApplication3.DTOs.ImageDto;
using WebApplication3.DTOs.Member;
using WebApplication3.DTOs.Sponsor;

namespace WebApplication3.DTOs.Project
{
    public class ProjectDTO
    {
        [JsonIgnore]
        public int Project_id { get; set; }
        public string Name { get; set; }
        public decimal Budget { get; set; }

        [Column(TypeName = "TEXT")]
        public string Description { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<MemberDTO>? Member { get; set; }
        public List<SponsorDTO>? Sponsor { get; set; }

        public List<ImageDtos>? images { get; set; }
    }
}
