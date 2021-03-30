using System;
using System.Collections.Generic;
using System.Threading;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Safir.Agent.Protos;

namespace Safir.Agent.Client
{
    internal class DefaultFilesClient : IFilesClient
    {
        private readonly FileSystem.FileSystemClient _client;
        private readonly ILogger<DefaultFilesClient> _logger;

        public DefaultFilesClient(FileSystem.FileSystemClient client, ILogger<DefaultFilesClient> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger;
        }
        
        public IAsyncEnumerable<FileSystemEntry> ListAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Making request to list files from agent");
            var streamingCall = _client.List(new Empty(), null, null, cancellationToken);
            _logger.LogDebug("Received response from agent");
            _logger.LogTrace("Reading response stream as an async enumerable");
            return streamingCall.ResponseStream.ReadAllAsync(cancellationToken);
        }
    }
}
