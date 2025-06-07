using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restauracja_wpf.Models
{
    [Table("Discounts")]
    public class Discount : DomainObject
    {
        //public int DiscountId { get; set; }

        [MaxLength(15)]
        public string LevelName { get; set; } = "";

        [Column(TypeName = "decimal(3, 2)")]
        public decimal DiscountMult { get; set; }

        [Column(TypeName = "smallint")]
        public short OrderThreshold { get; set; }

        public List<RegularClient> RegularCustomers { get; set; } = new List<RegularClient>();
    }
}
