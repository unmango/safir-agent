using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace Safir.Agent.Services
{
    internal sealed class RedisService : IHostedService
    {
        private IConnectionMultiplexer? _connection;
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var configuration = new ConfigurationOptions();
            _connection = await ConnectionMultiplexer.ConnectAsync(configuration);
            
            
                
            throw new System.NotImplementedException();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_connection != null)
            {
                await _connection.CloseAsync();
                _connection.Dispose();
            }
            
            throw new System.NotImplementedException();
        }
    }
}
