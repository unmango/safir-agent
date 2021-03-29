using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Safir.Agent.Configuration;
using Safir.Agent.Domain;

namespace Safir.Agent.Services
{
    internal class FilesService : Files.FilesBase
    {
        private readonly IOptions<AgentOptions> _options;
        private readonly IDirectory _directory;
        private readonly IFile _file;
        private readonly ILogger<FilesService> _logger;

        public FilesService(
            IOptions<AgentOptions> options,
            IDirectory directory,
            IFile file,
            ILogger<FilesService> logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _directory = directory ?? throw new ArgumentNullException(nameof(directory));
            _file = file ?? throw new ArgumentNullException(nameof(file));
            _logger = logger;
        }

        public override async Task List(
            Empty request,
            IServerStreamWriter<File> responseStream,
            ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(_options.Value.Root))
            {
                _logger.LogDebug("No root configured");
                return;
            }
            
            var entries = _directory.EnumerateFileSystemEntries(_options.Value.Root);

            foreach (var file in GetFiles(entries, _file))
            {
                await responseStream.WriteAsync(new File {
                    Path = file,
                });
            }
        }

        private IEnumerable<string> GetFiles(IEnumerable<string> entries, int maxDepth)
        {
        }
    }
}
