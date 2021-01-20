using Microsoft.EntityFrameworkCore;
using Core.Models;
using Core.Contracts.Repositories;

namespace Infrastructure.Data.Repositories
{
    public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
    {
        public ReservationRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
        {

        }
    }
}
