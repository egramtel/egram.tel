using System.Reactive.Disposables;
using ReactiveUI;

namespace Tel.Egram.Components.Messenger.Explorer.Messages
{
    public class StickerMessageModel : MessageModel, ISupportsActivation
    {
        public string Text { get; set; }
        
        public StickerMessageModel()
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