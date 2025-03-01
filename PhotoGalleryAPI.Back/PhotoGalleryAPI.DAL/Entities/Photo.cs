using System.ComponentModel.DataAnnotations;
namespace PhotoGalleryAPI.DAL.Entities
{
    public class Photo : BaseEntity
    {
        public string Filename { get; set; }

        public DateTime UploadedDate { get; set; } = DateTime.UtcNow;

        public Guid AlbumId { get; set; }
        public Album Album { get; set; }

        public virtual ICollection<Like> Likes { get; set; }
    }
}
