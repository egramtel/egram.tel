using System.Reactive.Disposables;
using PropertyChanged;
using ReactiveUI;
using Tel.Egram.Services.Graphics.Avatars;
using Tel.Egram.Services.Messaging.Chats;

namespace Tel.Egram.Model.Messenger.Informer
{
    [AddINotifyPropertyChangedInterface]
    public class InformerModel : ISupportsActivation
    {
        public bool IsVisible { get; set; } = true;
        
        public string Title { get; set; }
        
        public string Label { get; set; }
        
        public Avatar Avatar { get; set; }
        
        public InformerModel(Chat chat)
        {
            this.WhenActivated(disposables =>
            {
                this.BindInformer(chat)
                    .DisposeWith(disposables);
            });
        }

        public InformerModel(Aggregate aggregate)
        {
            this.WhenActivated(disposables =>
            {
                this.BindInformer(aggregate)
                    .DisposeWith(disposables);
            });
        }

        private InformerModel()
        {
        }
        
        public static InformerModel Hidden()
        {
            return new InformerModel
            {
                IsVisible = false
            };
        }
        
        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}