using System.ComponentModel.DataAnnotations;

namespace PhotoGalleryAPI.DAL.Entities
{
    public class Like : BaseEntity
    {
        public bool IsLike { get; set; } // true for like, false for dislike

        public Guid PhotoId { get; set; }
        public virtual Photo Photo { get; set; }

        public Guid PersonId { get; set; }
        public virtual Person Person { get; set; }
    }
}
