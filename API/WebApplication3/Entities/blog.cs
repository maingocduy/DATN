using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Entities
{
    public class blog
    {
        [Key, Column(Order = 1)]
        public int Blog_id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? CreatedAt { get; set; }

        public Project? Project { get; set; }

        public Member? Member { get; set; }
    }
}
