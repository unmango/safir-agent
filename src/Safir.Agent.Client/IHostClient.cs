using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Safir.Agent.Protos;

namespace Safir.Agent.Client
{
    [PublicAPI]
    public interface IHostClient
    {
        Task<HostInfo> GetHostInfoAsync(CancellationToken cancellationToken = default);
    }
}
