using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoGalleryAPI.Shared.DTOs
{
    public class PhotoDTO : BaseDTO
    {
        public string Filename { get; set; }
        public DateTime UploadedDate { get; set; }
        public string AlbumId { get; set; }
        public List<LikeDTO> Likes { get; set; } = new List<LikeDTO>();
    }
}
