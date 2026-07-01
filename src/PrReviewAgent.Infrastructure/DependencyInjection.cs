using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OllamaSharp;
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

            var ollamaBaseUrl = configuration["Ollama:BaseUrl"] ?? "http://localhost:11434";
            var ollamaModel = configuration["Ollama:Model"] ?? "qwen3:latest";
            services.AddSingleton<IChatClient>(
                new OllamaApiClient(new Uri(ollamaBaseUrl)) { SelectedModel = ollamaModel });

            services.AddScoped<IPrService, FakePrService>();
            services.AddScoped<IAiReviewService, OllamaReviewService>();
            return services;
        }
    }
}
