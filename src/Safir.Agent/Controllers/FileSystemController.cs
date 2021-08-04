using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Safir.Agent.Configuration;
using Safir.Agent.Protos;
using Safir.Agent.Queries;

namespace Safir.Agent.Controllers
{
    [ApiController]
    internal class FileSystemController : ControllerBase
    {
        private readonly IOptions<AgentOptions> _options;
        private readonly ISender _sender;
        private readonly ILogger<FileSystemController> _logger;

        public FileSystemController(IOptions<AgentOptions> options, ISender sender, ILogger<FileSystemController> logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _sender = sender ?? throw new ArgumentNullException(nameof(sender));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IEnumerable<FileSystemEntry>> List()
        {
            var root = _options.Value.DataDirectory;
            if (string.IsNullOrWhiteSpace(root))
            {
                _logger.LogInformation("No data directory set, returning");
                return Enumerable.Empty<FileSystemEntry>();
            }

            var enumerationOptions = _options.Value.EnumerationOptions;
            _logger.LogTrace("Sending list files request");
            var result = await _sender.Send(new ListFilesRequest(root, enumerationOptions));
            _logger.LogTrace("Got list files response");

            _logger.LogTrace("Returning files list");
            return result.Files;
        }
    }
}
