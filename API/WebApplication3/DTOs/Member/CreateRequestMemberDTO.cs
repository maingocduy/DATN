﻿using System.ComponentModel.DataAnnotations;

namespace WebApplication3.DTOs.Member
{
    public class CreateRequestMemberDTO
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone is required")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Group_id is required")]
        public int Group_id { get; set; }

        public string nameProject { get; set; }
    }
}
