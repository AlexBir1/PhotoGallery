using System.ComponentModel.DataAnnotations;

namespace PhotoGalleryAPI.DAL.Entities
{
    public class Album : BaseEntity
    {
        public string Title { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public Guid CreatedByPersonId { get; set; }
        public virtual Person CreatedBy { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }
    }
}
