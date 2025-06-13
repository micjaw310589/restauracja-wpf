using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restauracja_wpf.Models
{
    [Table("Menu")]
    public class Dish : DomainObject
    {
        //public int DishId { get; set; }

        [MaxLength(20)]
        public string Name { get; set; } = "";

        [Column(TypeName = "decimal(5, 2)")]
        public decimal Price { get; set; }

        public bool Available { get; set; } = true;

        [Column(TypeName = "time")]
        public TimeSpan? TimeConstant { get; set; } = null;

        [Column(TypeName = "time")]
        public TimeSpan? TimeCalculated { get; set; } = null;
        public bool IsTimeCalculated { get; set; } = false;

        public bool DishOfTheDay { get; set; } = false;

        public bool Exclude { get; set; } = false;

        public List<DishOrder> DishOrders { get; set; } = new List<DishOrder>();
    }
}
