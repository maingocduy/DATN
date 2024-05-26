﻿using System.Text.Json.Serialization;
using WebApplication3.DTOs.Member;
using WebApplication3.DTOs.Project;

namespace WebApplication3.DTOs.Blog
{
    public class BlogDTO
    {
        [JsonIgnore]
        public int? Blog_id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? CreatedAt { get; set; }

        public ProjectDTO? project { get; set; }

        public MemberDTO? Member { get; set; }
    }
}
