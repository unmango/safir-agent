using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using Safir.Agent.Protos;

namespace Safir.Agent.Client
{
    [PublicAPI]
    public interface IFileSystemClient
    {
        IAsyncEnumerable<FileSystemEntry> ListAsync(CancellationToken cancellationToken = default);
    }
}
