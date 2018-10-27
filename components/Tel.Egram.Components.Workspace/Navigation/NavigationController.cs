using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Avalonia.Threading;
using Tel.Egram.Graphics;
using Tel.Egram.Gui.Views.Workspace;
using Tel.Egram.Gui.Views.Workspace.Navigation;
using Tel.Egram.Messaging.Users;

namespace Tel.Egram.Components.Workspace.Navigation
{
    public class NavigationController : BaseController<NavigationControlModel>
    {
        public NavigationController(
            IAvatarLoader avatarLoader,
            IUserLoader userLoader)
        {
            BindAvatar(userLoader, avatarLoader)
                .DisposeWith(this);
        }
        
        private IDisposable BindAvatar(
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
                    Model.Avatar = avatar;
                });
        }
    }
}