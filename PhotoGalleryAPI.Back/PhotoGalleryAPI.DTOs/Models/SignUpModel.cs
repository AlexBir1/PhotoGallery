using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoGalleryAPI.Shared.Models
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [MinLength(8, ErrorMessage = "Minimum password characters: 8")]
        public string Password { get; set; } = string.Empty;

        [Compare("Password", ErrorMessage = "Passwords must match")]
        public string PasswordConfirm { get; set; } = string.Empty;

        public bool KeepAuthorized { get; set; } = false;
    }
}
