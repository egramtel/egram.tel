using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Avalonia.Threading;
using Tel.Egram.Graphics;
using Tel.Egram.Gui.Views.Workspace;
using Tel.Egram.Messaging.Users;

namespace Tel.Egram.Components.Workspace.Navigation
{
    public class NavigationController
        : BaseController, INavigationController
    {
        public NavigationController(
            NavigationControlModel model,
            IAvatarLoader avatarLoader,
            IUserLoader userLoader)
        {
            BindAvatar(model, userLoader, avatarLoader)
                .DisposeWith(this);
        }
        
        private IDisposable BindAvatar(
            NavigationControlModel model,
            IUserLoader userLoader,
            IAvatarLoader avatarLoader)
        {
            return userLoader
                .GetMe()
                .SelectMany(user => avatarLoader.LoadAvatar(user.UserData))
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(AvaloniaScheduler.Instance)
                .Subscribe(avatar =>
                {
                    model.Avatar = avatar;
                });
        }
    }
}