using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using Core.Contracts.Repositories;
using Core.Contracts.Services;

using Infrastructure.Data;
using Infrastructure.Data.Repositories;

using Core.Business.Services;

namespace Ioc.Resolver
{
    public static class IoCRegister
    {
        public static IServiceCollection ConfigureIoC(this IServiceCollection services, IConfiguration configuration)
        {
            // Data access
            services.AddDbContextFactory<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlConnection"));
            });

            services.AddTransient<IRoomRepository, RoomRepository>();
            services.AddTransient<IReservationRepository, ReservationRepository>();
            services.AddTransient<IReservationStateRepository, ReservationStateRepository>();

            // Business
            services.AddTransient<IRoomService, RoomService>();
            services.AddTransient<IReservationService, ReservationService>();
            services.AddTransient<IReservationStateService, ReservationStateService>();

            return services;
        }

        public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var contextFactory = serviceScope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
            using var context = contextFactory.CreateDbContext();
            context.Database.Migrate();

            return app;
        }
    }
}
