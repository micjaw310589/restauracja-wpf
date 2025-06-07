using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restauracja_wpf.Models
{
    [Table("Orders")]
    public class Order : DomainObject
    {
        //public int OrderId { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; } = null;

        public int? ReservationId { get; set; }
        public Reservation? Reservation { get; set; } = null;

        public int? RegularCustomerId { get; set; }
        public RegularClient? RegularCustomer { get; set; } = null;

        public int TableId { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? OrderDate { get; set; } = DateTime.Now;

        [Column(TypeName = "datetime")]
        public DateTime? DeliveryDate { get; set; } = null;

        [Column(TypeName = "tinyint")]
        public sbyte? DeliveryNumber { get; set; } = null;

        //[Column(TypeName = "tinyint")]
        public int StatusId { get; set; }
        public OrderStatus Status { get; set; } = null!;

        public List<DishOrder> DishOrders { get; set; } = new List<DishOrder>();
        public List<Table> Tables { get; set; } = new List<Table>();
    }
}
