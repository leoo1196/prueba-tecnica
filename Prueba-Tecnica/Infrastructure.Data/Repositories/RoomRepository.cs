using Microsoft.EntityFrameworkCore;
using Core.Models;
using Core.Contracts.Repositories;

namespace Infrastructure.Data.Repositories
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        public RoomRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
        {

        }
    }
}
