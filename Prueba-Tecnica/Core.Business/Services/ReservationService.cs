using System;
using System.Threading.Tasks;
using System.Linq;
using Core.Models;
using Core.Models.Enums;
using Core.Contracts.Repositories;
using Core.Contracts.Services;
using System.Collections.Generic;

namespace Core.Business.Services
{
    public class ReservationService : GenericService<Reservation>, IReservationService
    {
        public ReservationService(IReservationRepository repository) : base(repository)
        {

        }

        public override async Task CreateAsync(Reservation entity)
        {
            entity.IdReservationState = (int)ReservationStatesEnum.Pendiente;
            entity.CreationDate = DateTime.Now;
            await _repository.Insert(entity);
        }

        public async Task<Reservation> GetLastReservationByRoomAsync(int idRoom)
        {
            var reservations = await GetAllAsync();

            return reservations
                .Where(e => e.IdRoom == idRoom && e.IdReservationState == (int)ReservationStatesEnum.Pendiente)
                .OrderByDescending(e => e.CreationDate)
                .FirstOrDefault();
        }

        public async Task<ICollection<Reservation>> GetReservationsAsync(DateTime initDate, DateTime finishDate)
        {
            return await _repository
                .GetEntities(e => e.CreationDate >= initDate && e.CreationDate <= finishDate && e.IdRoom.HasValue, "Room");
        }
    }
}
