using System.Collections.Generic;
using System.Threading;

namespace Safir.Agent.Client
{
    public interface IFilesClient
    {
        IAsyncEnumerable<File> ListAsync(CancellationToken cancellationToken = default);
    }
}
