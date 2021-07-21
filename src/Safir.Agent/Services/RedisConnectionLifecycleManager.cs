using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Safir.Agent.Services
{
    internal sealed class RedisConnectionLifecycleManager : LifecycleManager<ConnectionMultiplexer>
    {
        private readonly IOptionsMonitor<RedisCacheOptions> _options;

        public RedisConnectionLifecycleManager(IOptionsMonitor<RedisCacheOptions> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }
        
        protected override async Task<ConnectionMultiplexer> CreateAsync(string name)
        {
            var options = _options.Get(name).Configuration;
            var configuration = ConfigurationOptions.Parse(options);
            return await ConnectionMultiplexer.ConnectAsync(configuration);
        }
    }
}
