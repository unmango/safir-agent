using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using Safir.Agent.Domain;

namespace Safir.Agent.Queries
{
    [UsedImplicitly]
    internal sealed class ListFilesValidator : IPipelineBehavior<ListFilesRequest, ListFilesResponse>
    {
        private readonly IDirectory _directory;
        private readonly ILogger<ListFilesValidator> _logger;

        public ListFilesValidator(IDirectory directory, ILogger<ListFilesValidator> logger)
        {
            _directory = directory ?? throw new ArgumentNullException(nameof(directory));
            _logger = logger;
        }
        
        public Task<ListFilesResponse> Handle(
            ListFilesRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<ListFilesResponse> next)
        {
            var root = request.Root;
            if (string.IsNullOrWhiteSpace(root))
            {
                _logger.LogDebug("No root configured");
                return ListFilesResponse.EmptyTask;
            }

            // ReSharper disable once InvertIf
            if (!_directory.Exists(root))
            {
                _logger.LogError("Data directory doesn't exist");
                return ListFilesResponse.EmptyTask;
            }

            return next();
        }
    }
}
