using System.Reactive.Disposables;
using ReactiveUI;

namespace Tel.Egram.Model.Messenger.Explorer.Messages.Special
{
    public class DocumentMessageModel : MessageModel, ISupportsActivation
    {
        public bool IsDownloaded { get; set; }
        
        public string Name { get; set; }
        
        public string Text { get; set; }
        
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
            });
        }
        
        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}