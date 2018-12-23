using System.Reactive.Disposables;
using ReactiveUI;
using TdLib;

namespace Tel.Egram.Components.Messenger.Explorer.Messages.Visual
{
    public class PhotoMessageModel : VisualMessageModel, ISupportsActivation
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