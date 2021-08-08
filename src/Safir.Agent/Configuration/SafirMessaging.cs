using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Safir.Messaging.Configuration;

namespace Safir.Agent.Configuration
{
    public class SafirMessaging : IConfigureOptions<MessagingOptions>
    {
        private readonly IConfiguration _configuration;

        public SafirMessaging(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(MessagingOptions options)
        {
            options.ConnectionString = _configuration["Redis"];
        }
    }
}
