using System;
using Grpc.Net.ClientFactory;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Safir.Agent.Protos;

namespace Safir.Agent.Client.DependencyInjection
{
    [PublicAPI]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSafirAgentClient(this IServiceCollection services)
        {
            services.AddLogging();
            services.AddHttpClient();
            services.AddOptions<AgentClientOptions>();

            services.AddTransient<IFileSystemClient, DefaultFileSystemClient>();
            services.AddGrpcClient<FileSystem.FileSystemClient>(ConfigureGrpcClient);

            services.AddTransient<IHostClient, DefaultHostClient>();
            services.AddGrpcClient<Host.HostClient>(ConfigureGrpcClient);

            return services;
        }

        public static IServiceCollection AddSafirAgentClient(
            this IServiceCollection services,
            Action<AgentClientOptions> configure)
        {
            return services.Configure(configure).AddSafirAgentClient();
        }

        private static void ConfigureGrpcClient(IServiceProvider services, GrpcClientFactoryOptions options)
        {
            var clientOptions = services.GetRequiredService<IOptions<AgentClientOptions>>();
            options.Address = new Uri(clientOptions.Value.BaseUrl);
        }
    }
}
