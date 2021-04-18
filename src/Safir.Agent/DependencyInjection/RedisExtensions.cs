using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Core.Implementations;

namespace Safir.Agent.DependencyInjection
{
    internal static class RedisExtensions
    {
        public static IServiceCollection AddRedisClient(
            this IServiceCollection services,
            Action<RedisConfiguration> configure)
        {
            // Potentially optional... Serializer?
            services.AddSingleton<IRedisCacheClient, RedisCacheClient>();
            services.AddSingleton<IRedisCacheConnectionPoolManager, RedisCacheConnectionPoolManager>();
            services.Configure(configure);
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<RedisConfiguration>>().Value);
            
            return services;
        }
    }
}
