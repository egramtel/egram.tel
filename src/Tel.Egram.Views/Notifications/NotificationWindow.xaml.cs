using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using Tel.Egram.Model.Notifications;

namespace Tel.Egram.Views.Notifications
{
    public class NotificationWindow : BaseWindow<NotificationModel>
    {
        private static NotificationWindow _current;
        
        public NotificationWindow() : base(false)
        {
            this.WhenActivated(disposables =>
            {
                this.BindAutohide()
                    .DisposeWith(disposables);
            });
            
            AvaloniaXamlLoader.Load(this);
        }

        public override void Show()
        {   
            _current?.Close();
            
            base.Show();

            _current = this;
        }

        protected override void HandleClosed()
        {
            _current = null;
            
            base.HandleClosed();
        }
    }
}
