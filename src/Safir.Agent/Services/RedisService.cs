using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace Safir.Agent.Services
{
    internal sealed class RedisService : IHostedService
    {
        private readonly ILifecycleManager<ConnectionMultiplexer> _connectionManager;
        private IConnectionMultiplexer? _connection;

        public RedisService(ILifecycleManager<ConnectionMultiplexer> connectionManager)
        {
            _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _connection = await _connectionManager.GetOrCreateAsync(string.Empty);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_connection == null) return;
            
            await _connection.CloseAsync();
            _connection.Dispose();
        }
    }
}
