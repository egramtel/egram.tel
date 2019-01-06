using System.Reactive.Disposables;
using ReactiveUI;
using TdLib;

namespace Tel.Egram.Model.Messenger.Explorer.Messages.Visual
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