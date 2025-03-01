using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoGalleryAPI.Shared.Models
{
    public class SignInModel
    {
        [Required(ErrorMessage = "Username, email is required")]
        public string UserIdentifier { get; set; } = string.Empty;

        [MinLength(8, ErrorMessage = "Minimum password characters: 8")]
        public string Password { get; set; } = string.Empty;

        public bool KeepAuthorized { get; set; } = false;
    }
}
