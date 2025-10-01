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
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<IPurchaseService, PurchaseService>();
            services.AddScoped<ICheckpointService, CheckpointService>();    
        }

        private static void SetupInfrastructure(IServiceCollection services)
        {
            services.AddScoped(typeof(ITourRepository), typeof(TourRepository));
            services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            services.AddScoped(typeof(ICheckpointRepository), typeof(CheckpointRepository));
            services.AddScoped(typeof(ITagRepository), typeof(TagRepository));

            services.AddDbContext<ToursContext>(opt =>
                opt.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"), x => x.MigrationsHistoryTable("__EFMigrationsHistory", "tours")));
        }
    }
}

