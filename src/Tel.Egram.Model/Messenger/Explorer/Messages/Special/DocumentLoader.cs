using System;
using System.IO;
using System.Reactive.Linq;
using ReactiveUI;
using Splat;
using Tel.Egram.Services.Persistance;

namespace Tel.Egram.Model.Messenger.Explorer.Messages.Special
{
    /// <summary>
    /// File loading logic for documents
    /// </summary>
    public class DocumentLoader
    {
        private readonly IFileLoader _fileLoader;

        public DocumentLoader(
            IFileLoader fileLoader)
        {
            _fileLoader = fileLoader;
        }
        
        public DocumentLoader()
            : this(
                Locator.Current.GetService<IFileLoader>())
        {
        }
        
        public IDisposable Bind(
            DocumentMessageModel model)
        {
            model.DownloadCommand = ReactiveCommand.CreateFromObservable(
                (DocumentMessageModel m) => Download(m), null, RxApp.MainThreadScheduler);

            var file = model.Document.Document_;
            model.IsDownloaded = (file.Local?.IsDownloadingCompleted ?? false)
                                 && File.Exists(file.Local?.Path);
            
            return model.DownloadCommand.Subscribe(isDownloaded =>
            {
                model.IsDownloaded = isDownloaded;
            });
        }
        
        private IObservable<bool> Download(
            DocumentMessageModel model)
        {
            var file = model.Document.Document_;

            return _fileLoader.LoadFile(file, LoadPriority.Mid)
                .FirstAsync(f => f.Local != null && f.Local.IsDownloadingCompleted)
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Select(f => f.Local.IsDownloadingCompleted);
        }
    }
}