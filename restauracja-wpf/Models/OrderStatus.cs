using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restauracja_wpf.Models
{
    [Table("Status")]
    public class OrderStatus : DomainObject
    {
        [Column(TypeName = "tinyint")]
        public sbyte StatusId { get; set; }

        [MaxLength(15)]
        public string Name { get; set; } = "";

        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
