using System.IO;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Moq.AutoMock;
using Safir.Agent.Events;
using Safir.Agent.Services;
using Xunit;

namespace Safir.Agent.Tests.Services
{
    public class FileEventPublisherTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly FileEventPublisher _service;

        public FileEventPublisherTests()
        {
            _service = _mocker.CreateInstance<FileEventPublisher>();
        }

        [Fact]
        public async Task PublishesCreatedEventsWhenStarted()
        {
            var subject = new Subject<FileSystemEventArgs>();
            var watcher = _mocker.GetMock<IFileWatcher>();
            var publisher = _mocker.GetMock<IPublisher>();
            watcher.SetupGet(x => x.Created).Returns(subject);
            watcher.SetupGet(x => x.Changed).Returns(new Subject<FileSystemEventArgs>());
            watcher.SetupGet(x => x.Deleted).Returns(new Subject<FileSystemEventArgs>());
            watcher.SetupGet(x => x.Renamed).Returns(new Subject<RenamedEventArgs>());
            const string name = "name";

            await _service.StartAsync(default);
            subject.OnNext(new(WatcherChangeTypes.Created, "/dir", name));
            
            publisher.Verify(x => x.Publish<INotification>(
                It.Is<FileCreated>(e => e.Path == name),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task PublishesChangedEventsWhenStarted()
        {
            var subject = new Subject<FileSystemEventArgs>();
            var watcher = _mocker.GetMock<IFileWatcher>();
            var publisher = _mocker.GetMock<IPublisher>();
            watcher.SetupGet(x => x.Created).Returns(new Subject<FileSystemEventArgs>());
            watcher.SetupGet(x => x.Changed).Returns(subject);
            watcher.SetupGet(x => x.Deleted).Returns(new Subject<FileSystemEventArgs>());
            watcher.SetupGet(x => x.Renamed).Returns(new Subject<RenamedEventArgs>());
            const string name = "name";

            await _service.StartAsync(default);
            subject.OnNext(new(WatcherChangeTypes.Changed, "/dir", name));
            
            publisher.Verify(x => x.Publish<INotification>(
                It.Is<FileChanged>(e => e.Path == name),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task PublishesDeletedEventsWhenStarted()
        {
            var subject = new Subject<FileSystemEventArgs>();
            var watcher = _mocker.GetMock<IFileWatcher>();
            var publisher = _mocker.GetMock<IPublisher>();
            watcher.SetupGet(x => x.Created).Returns(new Subject<FileSystemEventArgs>());
            watcher.SetupGet(x => x.Changed).Returns(new Subject<FileSystemEventArgs>());
            watcher.SetupGet(x => x.Deleted).Returns(subject);
            watcher.SetupGet(x => x.Renamed).Returns(new Subject<RenamedEventArgs>());
            const string name = "name";

            await _service.StartAsync(default);
            subject.OnNext(new(WatcherChangeTypes.Deleted, "/dir", name));
            
            publisher.Verify(x => x.Publish<INotification>(
                It.Is<FileDeleted>(e => e.Path == name),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task PublishesRenamedEventsWhenStarted()
        {
            var subject = new Subject<RenamedEventArgs>();
            var watcher = _mocker.GetMock<IFileWatcher>();
            var publisher = _mocker.GetMock<IPublisher>();
            watcher.SetupGet(x => x.Created).Returns(new Subject<FileSystemEventArgs>());
            watcher.SetupGet(x => x.Changed).Returns(new Subject<FileSystemEventArgs>());
            watcher.SetupGet(x => x.Deleted).Returns(new Subject<FileSystemEventArgs>());
            watcher.SetupGet(x => x.Renamed).Returns(subject);
            const string name = "name", oldName = "old";

            await _service.StartAsync(default);
            subject.OnNext(new(WatcherChangeTypes.Renamed, "/dir", name, oldName));
            
            publisher.Verify(x => x.Publish<INotification>(
                It.Is<FileRenamed>(e => e.Path == name && e.OldPath == oldName),
                It.IsAny<CancellationToken>()));
        }
    }
}
