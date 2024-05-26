using System.Text.Json.Serialization;

namespace WebApplication3.DTOs.Groups
{
    public class GroupsDTOs
    {
        [JsonIgnore]
        public int Groups_id { get; set; }
        public string group_name { get; set; }
    }
}
