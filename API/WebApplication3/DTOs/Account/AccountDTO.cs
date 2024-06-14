using System.Text.Json.Serialization;
using WebApplication3.DTOs.Groups;
using WebApplication3.DTOs.Member;
using WebApplication3.Entities;

namespace WebApplication3.DTOs.Account
{
    public class AccountDTO
    {
        public string Username { get; set; }
        [JsonIgnore]        
        public string Password { get; set; }

        public MemberDTO Member { get; set; } = new MemberDTO();

    }
}
