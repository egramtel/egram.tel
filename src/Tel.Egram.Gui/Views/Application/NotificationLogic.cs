using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using ReactiveUI;
using Splat;
using Tel.Egram.Components.Notifications;
using Tel.Egram.Gui.Views.Notifications;
using Tel.Egram.Utils.Platforms;

namespace Tel.Egram.Gui.Views.Application
{
    public static class NotificationLogic
    {
        public static IDisposable BindNotifications(
            this MainWindow mainWindow)
        {
            return mainWindow.BindNotifications(
                Locator.Current.GetService<IPlatform>(),
                Locator.Current.GetService<INotificationController>());
        }
        
        public static IDisposable BindNotifications(
            this MainWindow mainWindow,
            IPlatform platform,
            INotificationController controller)
        {
            var trigger = (controller as NotificationController)?.Trigger;

            if (trigger != null)
            {
                return trigger
                    .SubscribeOn(RxApp.TaskpoolScheduler)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(model =>
                    {
                        var screen = mainWindow.Screens.Primary;
                        var window = new NotificationWindow();
                        window.Show();
                        
                        window.DataContext = model;
                        window.Position = new Point(
                            GetXForNotification(platform, screen.Bounds, window.Bounds),
                            GetYForNotification(platform, screen.Bounds, window.Bounds));
                    });
            }

            return Disposable.Empty;
        }

        private static double GetXForNotification(IPlatform platform, Rect outer, Rect inner)
        {
            switch (platform)
            {
                case WindowsPlatform _:
                    return outer.Width - inner.Width;
                default:
                    return outer.Width - inner.Width;
            }
        }

        private static double GetYForNotification(IPlatform platform, Rect outer, Rect inner)
        {
            switch (platform)
            {
                case WindowsPlatform _:
                    return outer.Height - inner.Height;
                default:
                    return 0.0;
            }
        }
    }
}