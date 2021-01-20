using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    [Table("Rooms")]
    public class Room
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(200)"), Required]
        public string Name { get; set; }

        public int Capacity { get; set; }

        public bool HasProjector { get; set; }

        public bool HasBlackboard { get; set; }

        public bool HasInternet { get; set; }

        public bool IsReserved { get; set; }

        public DateTime CreationDate { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}
