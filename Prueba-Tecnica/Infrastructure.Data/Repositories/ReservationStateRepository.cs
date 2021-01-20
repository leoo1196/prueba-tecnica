using Microsoft.EntityFrameworkCore;
using Core.Models;
using Core.Contracts.Repositories;

namespace Infrastructure.Data.Repositories
{
    public class ReservationStateRepository : GenericRepository<ReservationState>, IReservationStateRepository
    {
        public ReservationStateRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
        {

        }
    }
}
