using System;
using System.Reactive.Disposables;
using ReactiveUI;
using TdLib;

namespace Tel.Egram.Model.Messenger.Explorer.Messages.Special
{
    public class DocumentMessageModel : MessageModel, ISupportsActivation
    {
        public TdApi.Document Document { get; set; }
        
        public bool IsDownloaded { get; set; }
        
        public string Name { get; set; }
        
        public string Text { get; set; }
        
        public string Size { get; set; }
        
        public ReactiveCommand<DocumentMessageModel, bool> DownloadCommand { get; set; }
        
        public ReactiveCommand<DocumentMessageModel, bool> ShowCommand { get; set; }
        
        public DocumentMessageModel()
        {       
            this.WhenActivated(disposables =>
            {
                this.BindAvatarLoading()
                    .DisposeWith(disposables);

                if (Reply != null)
                {
                    Reply.BindPreviewLoading()
                        .DisposeWith(disposables);
                }

                new DocumentLoader()
                    .Bind(this)
                    .DisposeWith(disposables);
                
                new DocumentExplorer()
                    .Bind(this)
                    .DisposeWith(disposables);
            });
        }
        
        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}