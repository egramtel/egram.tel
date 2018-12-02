using System.Reactive.Disposables;
using PropertyChanged;
using ReactiveUI;

namespace Tel.Egram.Components.Notifications
{
    [AddINotifyPropertyChangedInterface]
    public class NotificationModel : ISupportsActivation
    {
        public string Title { get; set; }
        
        public string Text { get; set; }
        
        public NotificationModel()
        {
            
        }
        
        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}