using System.Reactive.Disposables;
using ReactiveUI;
using TdLib;

namespace Tel.Egram.Components.Messenger.Explorer.Messages
{
    public class VideoMessageModel : PreviewableMessageModel, ISupportsActivation
    {
        public string Text { get; set; }
        
        public TdApi.Video VideoData { get; set; }
        
        public VideoMessageModel()
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