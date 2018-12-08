using System.Reactive.Disposables;
using PropertyChanged;
using ReactiveUI;
using Tel.Egram.Graphics;
using Tel.Egram.Messaging.Chats;

namespace Tel.Egram.Components.Messenger.Catalog.Entries
{
    [AddINotifyPropertyChangedInterface]
    public class EntryModel : ISupportsActivation
    {
        public long Id { get; set; }
        
        public int Order { get; set; }
        
        public string Title { get; set; }
        
        public Avatar Avatar { get; set; }

        public bool HasUnread { get; set; }

        public string UnreadCount { get; set; }

        public EntryModel()
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