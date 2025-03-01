using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoGalleryAPI.Shared.Models
{
    public class AuthorizationModel
    {
        public string PersonId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime TokenExpirationDate { get; set; }
        public bool KeepAuthorized { get; set; } = false;
    }
}
