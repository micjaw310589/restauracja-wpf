using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restauracja_wpf.Models
{
    [Table("Menu_Orders")]
    public class DishOrder : DomainObject
    {
        //public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public int DishId { get; set; }
        public Dish Dish { get; set; } = null!;

        [Column(TypeName = "decimal(5, 2)")]
        public decimal PurchasePrice { get; set; }

        public bool IsDiscountable { get; set; } = false;
    }
}
