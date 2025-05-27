using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restauracja_wpf.Models
{
    [Table("Reservations")]
    public class Reservation
    {
        public int ReservationId { get; set; }

        [MaxLength(20)]
        public string LastName { get; set; } = "";

        [MaxLength(12)]
        public string PhoneNumber { get; set; } = "";

        public int TableId { get; set; }
        public List<Table> Tables { get; set; } = new List<Table>();

        [Column(TypeName = "datetime")]
        public DateTime? ReservationDate { get; set; } = DateTime.Now;

        [Column(TypeName = "datetime")]
        public DateTime? EndDate { get; set; } = null;

        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
