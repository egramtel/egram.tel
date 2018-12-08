using System.Reactive.Disposables;
using DynamicData.Binding;
using PropertyChanged;
using ReactiveUI;
using Tel.Egram.Components.Messenger.Explorer.Messages;
using Tel.Egram.Messaging.Chats;

namespace Tel.Egram.Components.Messenger.Home
{
    [AddINotifyPropertyChangedInterface]
    public class HomeModel : ISupportsActivation
    {
        public bool IsVisible { get; set; } = true;
        
        public string SearchText { get; set; }
        
        public ObservableCollectionExtended<MessageModel> PromotedMessages { get; set; }
        
        public HomeModel()
        {
            this.WhenActivated(disposables =>
            {
                this.BindSearch()
                    .DisposeWith(disposables);

                this.BindPromoted()
                    .DisposeWith(disposables);
            });
        }
        
        public ViewModelActivator Activator => new ViewModelActivator();

        public static HomeModel Hidden()
        {
            return new HomeModel
            {
                IsVisible = false
            };
        }
    }
}