using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Input
{
    public class RoomModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, Range(1, 1000)]
        public int? Capacity { get; set; }

        [Required]
        public bool? HasProjector { get; set; }

        [Required]
        public bool? HasBlackboard { get; set; }

        [Required]
        public bool? HasInternet { get; set; }
    }
}
