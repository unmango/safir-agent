using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace Safir.Agent.Configuration
{
    public class GrpcWeb : IConfigureOptions<GrpcWebOptions>
    {
        public void Configure(GrpcWebOptions options)
        {
            options.DefaultEnabled = true;
        }
    }
}