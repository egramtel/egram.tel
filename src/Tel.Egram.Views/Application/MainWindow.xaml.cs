using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using Tel.Egram.Model.Application;

namespace Tel.Egram.Views.Application
{
    public class MainWindow : BaseWindow<MainWindowModel>
    {
        public MainWindow() : base(false)
        {
            this.WhenActivated(disposables =>
            {
                this.BindNotifications()
                    .DisposeWith(disposables);
            });
            
            AvaloniaXamlLoader.Load(this);
            this.AttachDevTools();
        }
    }
}
