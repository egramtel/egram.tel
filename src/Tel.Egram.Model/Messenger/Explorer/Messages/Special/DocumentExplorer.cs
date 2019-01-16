using System;
using System.IO;
using ReactiveUI;
using Splat;
using Tel.Egram.Services.Persistance;

namespace Tel.Egram.Model.Messenger.Explorer.Messages.Special
{
    /// <summary>
    /// Logic for showing document in explorer or finder
    /// </summary>
    public class DocumentExplorer
    {
        private readonly IFileExplorer _fileExplorer;

        public DocumentExplorer(
            IFileExplorer fileExplorer)
        {
            _fileExplorer = fileExplorer;
        }

        public DocumentExplorer()
            : this (
                Locator.Current.GetService<IFileExplorer>())
        {
        }
        
        public IDisposable Bind(
            DocumentMessageModel model)
        {
            model.ShowCommand = ReactiveCommand.Create(
                (DocumentMessageModel m) => Explore(m), null, RxApp.MainThreadScheduler);

            return model.ShowCommand.Subscribe();
        }
        
        private bool Explore(
            DocumentMessageModel model)
        {
            var localFile = model.Document.Document_?.Local;

            if (localFile?.Path != null)
            {
                var fileInfo = new FileInfo(localFile.Path);
                if (fileInfo.Exists)
                {
                    _fileExplorer.OpenDirectory(fileInfo);
                }
            }

            return false;
        }
    }
}