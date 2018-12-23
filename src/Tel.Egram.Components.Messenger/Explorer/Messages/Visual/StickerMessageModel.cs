using System.Reactive.Disposables;
using ReactiveUI;
using TdLib;

namespace Tel.Egram.Components.Messenger.Explorer.Messages.Visual
{
    public class StickerMessageModel : VisualMessageModel, ISupportsActivation
    {
        public TdApi.Sticker StickerData { get; set; }
        
        public StickerMessageModel()
        {
            this.WhenActivated(disposables =>
            {
                this.BindAvatarLoading()
                    .DisposeWith(disposables);

                this.BindPreviewLoading()
                    .DisposeWith(disposables);
            });
        }
        
        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}