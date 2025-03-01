using System.ComponentModel.DataAnnotations;
namespace PhotoGalleryAPI.DAL.Entities
{
    public class Role : BaseEntity
    {
        public string RoleName { get; set; }

        public virtual ICollection<Person> Persons { get; set; }
    }
}
