using Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Contracts.Services
{
    public interface IRoomService : IGenericeService<Room>
    {
        Task<Room> GetRoomWithReservationsAsync(int idRoom);

        Task<ICollection<Room>> SearchByResourcesAsync(int capacity, bool hasProjector, bool hasBlackboard, bool hasInternet);
    }
}
