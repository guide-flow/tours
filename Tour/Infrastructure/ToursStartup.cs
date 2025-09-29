using API.ServiceInterfaces;
using Core.Domain.RepositoryInterfaces;
using Core.Mappers;
using Core.UseCases;
using Infrastructure.Database;
using Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class ToursStartup
    {
        public static IServiceCollection ConfigureTours(this IServiceCollection services)
        {
            // Registers all profiles since it works on the assembly
            services.AddAutoMapper(typeof(ToursProfile));
            SetupCore(services);
            SetupInfrastructure(services);
            return services;
        }

        private static void SetupCore(IServiceCollection services)
        {
            services.AddScoped<ITourService,TourService>();
            services.AddScoped<IReviewService,ReviewService>();
        }

        private static void SetupInfrastructure(IServiceCollection services)
        {
            services.AddScoped(typeof(ITourRepository), typeof(TourRepository));
            services.AddScoped(typeof(IReviewRepository), typeof(ReviewRepository));

            services.AddDbContext<ToursContext>(opt =>
                opt.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"), x => x.MigrationsHistoryTable("__EFMigrationsHistory", "tours")));
        }
    }
}

