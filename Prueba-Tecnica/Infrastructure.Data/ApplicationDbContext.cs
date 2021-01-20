using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationState> ReservationStates { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
