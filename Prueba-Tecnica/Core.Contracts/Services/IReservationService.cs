using Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Contracts.Services
{
    public interface IReservationService : IGenericeService<Reservation>
    {
        Task<Reservation> GetLastReservationByRoomAsync(int idRoom);

        Task<ICollection<Reservation>> GetReservationsAsync(DateTime initDate, DateTime finishDate);
    }
}
