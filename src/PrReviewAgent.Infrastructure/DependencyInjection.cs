using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using PrReviewAgent.Application.Common.Interfaces;
using PrReviewAgent.Infrastructure.Services;
using PRReviewAgent.Infrastructure.Persistence;
using System.ClientModel;
using System.ClientModel.Primitives;

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

            var ollamaBaseUrl = configuration["Ollama:BaseUrl"] ?? "http://localhost:11434/v1";
            var ollamaModel = configuration["Ollama:Model"] ?? "qwen3:8b";
            var httpClient = new HttpClient { Timeout = TimeSpan.FromMinutes(5) };
            var openAiClient = new OpenAIClient(
                new ApiKeyCredential("ollama"),
                new OpenAIClientOptions
                {
                    Endpoint = new Uri(ollamaBaseUrl),
                    Transport = new HttpClientPipelineTransport(httpClient)
                });
            services.AddSingleton(openAiClient.GetChatClient(ollamaModel));

            services.AddScoped<IPrService, FakePrService>();
            services.AddScoped<IAiReviewService, OllamaReviewService>();
            return services;
        }
    }
}
