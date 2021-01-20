using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Core.Models;
using Core.Contracts.Repositories;
using Core.Contracts.Services;

namespace Core.Business.Services
{
    public class RoomService : GenericService<Room>, IRoomService
    {
        public RoomService(IRoomRepository repository) : base(repository)
        {

        }

        public override async Task CreateAsync(Room entity)
        {
            entity.CreationDate = DateTime.Now;
            await _repository.Insert(entity);
        }

        public async Task<Room> GetRoomWithReservationsAsync(int idRoom)
        {
            return (await _repository.GetEntities(e => e.Id == idRoom, "Reservations")).FirstOrDefault();
        }

        public async Task<ICollection<Room>> SearchByResourcesAsync(int capacity, bool hasProjector, bool hasBlackboard, bool hasInternet)
        {
            var rooms = await GetAllAsync();

            bool predicate(Room room) =>
                room.Capacity >= capacity &&
                (!hasProjector || room.HasProjector == hasProjector) &&
                (!hasBlackboard || room.HasBlackboard == hasBlackboard) &&
                (!hasInternet || room.HasInternet == hasInternet) &&
                !room.IsReserved;

            return rooms
                .Where(predicate)
                .OrderByDescending(e => e.Capacity)
                .ToList();
        }
    }
}
