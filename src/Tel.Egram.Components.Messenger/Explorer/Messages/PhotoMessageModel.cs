using System.Reactive.Disposables;
using ReactiveUI;
using TdLib;
using Tel.Egram.Graphics.Previews;

namespace Tel.Egram.Components.Messenger.Explorer.Messages
{
    public class PhotoMessageModel : PreviewableMessageModel, ISupportsActivation
    {
        public string Text { get; set; }
        
        public TdApi.Photo PhotoData { get; set; }
        
        public PhotoMessageModel()
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