using System.Reactive.Disposables;
using ReactiveUI;

namespace Tel.Egram.Components.Messenger.Explorer.Messages.Basic
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
            });
        }
        
        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}