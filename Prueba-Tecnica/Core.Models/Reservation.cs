using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    [Table("Reservations")]
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        public int NumberOfAssistants { get; set; }

        public bool UseProjector { get; set; }

        public bool UseBlackboard { get; set; }

        public bool UseInternet { get; set; }

        public int? IdRoom { get; set; }

        public int IdReservationState { get; set; }

        public DateTime CreationDate { get; set; }

        [ForeignKey("IdRoom")]
        public Room Room { get; set; }

        [ForeignKey("IdReservationState")]
        public ReservationState ReservationState { get; set; }
    }
}
