using EntityFrameworkCore.EncryptColumn.Attribute;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace restauracja_wpf.Models
{
    [Table("Users")]
    public class User : DomainObject
    {
        //public int UserId { get; set; }

        [MaxLength(20)]
        public string Login { get; set; } = "";

        [EncryptColumn]
        [MaxLength(50)]
        public string PasswordHash { get; set; } = "";

        [MaxLength(20)]
        public string FirstName { get; set; } = "";

        [MaxLength(30)]
        public string LastName { get; set; } = "";

        public int? RoleId { get; set; } = null;
        public Role? Role { get; set; } = null;

        public int? RestaurantId { get; set; } = null;
        public Restaurant? Restaurant { get; set; } = null;

        public bool Status { get; set; } = true;

        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
