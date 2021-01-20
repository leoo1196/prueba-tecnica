using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Input
{
    public class ReservationModel
    {
        [Required, Range(1, 1000)]
        public int? NumberOfAssistants { get; set; }

        [Required]
        public bool? UseProjector { get; set; }

        [Required]
        public bool? UseBlackboard { get; set; }

        [Required]
        public bool? UseInternet { get; set; }
    }
}
