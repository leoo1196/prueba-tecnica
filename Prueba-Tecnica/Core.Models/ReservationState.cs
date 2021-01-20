using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    [Table("ReservationStates")]
    public class ReservationState
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Description { get; set; }
    }
}
