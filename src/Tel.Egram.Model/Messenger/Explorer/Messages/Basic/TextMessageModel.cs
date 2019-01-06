using System.Reactive.Disposables;
using ReactiveUI;

namespace Tel.Egram.Model.Messenger.Explorer.Messages.Basic
{
    public class TextMessageModel : MessageModel, ISupportsActivation
    {
        public string Text { get; set; }
        
        public TextMessageModel()
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