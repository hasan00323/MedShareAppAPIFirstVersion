using Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Auth.Profile
{
    public class UserProfileDto
    {
        public string FullName { get; set; }
        public string? Email { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
