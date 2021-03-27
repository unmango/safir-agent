using System;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Options;
using Safir.Agent.Configuration;
using Safir.Agent.Domain;

namespace Safir.Agent.Services
{
    internal class FilesService : Files.FilesBase
    {
        private readonly IOptions<AgentOptions> _options;
        private readonly IFileSystem _fileSystem;

        public FilesService(IOptions<AgentOptions> options, IFileSystem fileSystem)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }
        
        public override async Task List(Empty request, IServerStreamWriter<File> responseStream, ServerCallContext context)
        {
            var files = System.IO.Directory.EnumerateFiles(_options.Value.Root);

            foreach (var file in files)
            {
                await responseStream.WriteAsync(new File {
                    Path = file,
                });
            }
        }
    }
}
