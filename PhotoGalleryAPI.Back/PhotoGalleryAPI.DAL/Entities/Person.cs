using System.ComponentModel.DataAnnotations;

namespace PhotoGalleryAPI.DAL.Entities
{
    public class Person : BaseEntity
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        public virtual ICollection<Album> Albums { get; set; }

    }
}
