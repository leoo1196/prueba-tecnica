using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models.Output
{
    public class ReservationModel
    {
        public int Id { get; set; }

        public int NumberOfAssistants { get; set; }

        public bool UseProjector { get; set; }

        public bool UseBlackboard { get; set; }

        public bool UseInternet { get; set; }

        public int IdRoom { get; set; }

        public int IdReservationState { get; set; }
    }
}
