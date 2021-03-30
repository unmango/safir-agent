using Microsoft.Extensions.DependencyInjection;
using Safir.Agent.Protos;

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
            services.AddGrpcClient<FileSystem.FileSystemClient>((sp, options) => {
                // TODO: Set service url
            });
            
            return services;
        }
    }
}
