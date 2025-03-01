using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoGalleryAPI.Shared.DTOs
{
    public class LikeDTO : BaseDTO
    {
        public bool IsLike { get; set; }
        public string PhotoId { get; set; }
        public string PersonId { get; set; }
    }
}
