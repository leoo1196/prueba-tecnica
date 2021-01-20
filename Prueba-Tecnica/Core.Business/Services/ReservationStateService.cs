using Core.Models;
using Core.Contracts.Repositories;
using Core.Contracts.Services;

namespace Core.Business.Services
{
    public class ReservationStateService : GenericService<ReservationState>, IReservationStateService
    {
        public ReservationStateService(IReservationStateRepository repository) : base(repository)
        {

        }
    }
}
