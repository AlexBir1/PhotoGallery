using System.ComponentModel.DataAnnotations;

namespace PhotoGalleryAPI.Shared.DTOs
{
    public class AlbumDTO : BaseDTO
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedByPersonId { get; set; }
        public List<PhotoDTO> Photos { get; set; } = new List<PhotoDTO>();
    }
}
