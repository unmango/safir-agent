using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Safir.Agent.Configuration;
using Safir.Agent.Events;

namespace Safir.Agent.Services
{
    internal sealed class DataDirectoryWatcher : IHostedService
    {
        private FileSystemWatcher? _fileWatcher;

        public DataDirectoryWatcher(
            IOptions<AgentOptions> options,
            IPublisher publisher,
            ILogger<DataDirectoryWatcher> logger)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            Publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            Logger = logger;
        }
        
        private CancellationTokenSource? TokenSource { get; set; }
        
        private IOptions<AgentOptions> Options { get; }
        
        private IPublisher Publisher { get; }
        
        private ILogger<DataDirectoryWatcher> Logger { get; }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogDebug("Starting data directory watcher");

            var root = Options.Value.DataDirectory;
            if (string.IsNullOrWhiteSpace(root))
            {
                Logger.LogInformation("No data directory set");
                return Task.CompletedTask;
            }

            Logger.LogTrace("Creating filesystem watcher");
            _fileWatcher = new FileSystemWatcher(root) {
                IncludeSubdirectories = true,
            };

            Logger.LogTrace("Assigning filesystem watcher event handlers");
            _fileWatcher.Created += OnCreated;
            _fileWatcher.Changed += OnChanged;
            _fileWatcher.Renamed += OnRenamed;
            _fileWatcher.Deleted += OnDeleted;
            _fileWatcher.Error += OnError;
            
            Logger.LogTrace("Creating cancellation token source");
            TokenSource = new CancellationTokenSource();

            Logger.LogTrace("Finishing data directory watcher start");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Stopping data directory watcher");

            if (TokenSource != null)
            {
                Logger.LogTrace("Disposing token source");
                TokenSource.Dispose();
            }

            if (_fileWatcher == null)
            {
                Logger.LogTrace("No file watcher created, returning");
                return Task.CompletedTask;
            }

            Logger.LogTrace("Removing filesystem watcher event handlers");
            _fileWatcher.Created -= OnCreated;
            _fileWatcher.Changed -= OnChanged;
            _fileWatcher.Renamed -= OnRenamed;
            _fileWatcher.Deleted -= OnDeleted;
            _fileWatcher.Error -= OnError;
            
            Logger.LogTrace("Finishing data directory watcher stop");
            return Task.CompletedTask;
        }

        private static async void OnCreated(object sender, FileSystemEventArgs e)
        {
            var notification = new FileCreated(e.FullPath);
            await SendAsync(sender, notification);
        }

        private static async void OnChanged(object sender, FileSystemEventArgs e)
        {
            var notification = new FileChanged(e.FullPath);
            await SendAsync(sender, notification);
        }

        private static async void OnRenamed(object sender, FileSystemEventArgs e)
        {
            var notification = new FileRenamed(e.FullPath);
            await SendAsync(sender, notification);
        }

        private static async void OnDeleted(object sender, FileSystemEventArgs e)
        {
            var notification = new FileDeleted(e.FullPath);
            await SendAsync(sender, notification);
        }

        private static void OnError(object sender, ErrorEventArgs e)
        {
            var service = (DataDirectoryWatcher)sender;
            service.Logger.LogError(e.GetException(), "Error in file watcher");
        }

        private static Task SendAsync(object sender, INotification notification)
        {
            var service = (DataDirectoryWatcher)sender;
            var token = service.TokenSource?.Token ?? default;
            
            return service.Publisher.Publish(notification, token);
        }
    }
}
