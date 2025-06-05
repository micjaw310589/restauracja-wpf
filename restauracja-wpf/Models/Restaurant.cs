using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restauracja_wpf.Models
{
    [Table("Restaurants")]
    public class Restaurant : DomainObject
    {
        public int RestaurantId { get; set; }

        [MaxLength(30)]
        public string Name { get; set; } = "";

        [MaxLength(50)]
        public string Address { get; set; } = "";

        [MaxLength(20)]
        public string City { get; set; } = "";

        public bool IsOpen { get; set; } = true;

        public List<User> Users { get; set; } = new List<User>();
        public List<Table> Tables { get; set; } = new List<Table>();
    }
}
