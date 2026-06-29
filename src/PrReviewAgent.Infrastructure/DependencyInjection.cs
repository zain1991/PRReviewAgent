using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrReviewAgent.Application.Common.Interfaces;
using PRReviewAgent.Infrastructure.Persistence;
using PrReviewAgent.Infrastructure.Services;

namespace PRReviewAgent.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IApplicationDbContext>(provider =>
                provider.GetRequiredService<AppDbContext>());

            services.AddScoped<IPrService, FakePrService>();
            return services;
        }
    }
}
