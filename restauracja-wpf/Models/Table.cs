using System.ComponentModel.DataAnnotations.Schema;

namespace restauracja_wpf.Models
{
    [Table("Tables")]
    public class Table : DomainObject
    {
        //public int TableId { get; set; }

        public int RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; } = null;

        public int TableNumber { get; set; }

        [Column(TypeName = "smallint")]
        public short Seats { get; set; }

        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
