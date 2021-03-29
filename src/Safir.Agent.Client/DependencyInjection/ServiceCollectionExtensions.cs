using Microsoft.Extensions.DependencyInjection;

namespace Safir.Agent.Client.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSafirAgentClient(this IServiceCollection services)
        {
            services.AddLogging();
            services.AddHttpClient();
            services.AddOptions();

            services.AddTransient<IFilesClient, DefaultFilesClient>();
            services.AddGrpcClient<Files.FilesClient>((sp, options) => {
                // TODO: Set service url
            });
            
            return services;
        }
    }
}
