using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restauracja_wpf.Models
{
    [Table("Roles")]
    public class Role : DomainObject
    {
        //public int RoleId { get; set; }

        [MaxLength(15)]
        public string Name { get; set; } = "";

        public List<User> Users { get; set; } = new List<User>();

    }
}
