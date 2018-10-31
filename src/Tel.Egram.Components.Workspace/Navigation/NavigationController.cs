using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Avalonia.Threading;
using ReactiveUI;
using Tel.Egram.Graphics;
using Tel.Egram.Messaging.Users;
using Tel.Egram.Models.Workspace.Navigation;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Workspace.Navigation
{
    public class NavigationController : Controller<NavigationModel>
    {
        public NavigationController(
            ISchedulers schedulers,
            IAvatarLoader avatarLoader,
            IUserLoader userLoader)
        {
            BindAvatar(schedulers, userLoader, avatarLoader)
                .DisposeWith(this);
        }
        
        private IDisposable BindAvatar(
            ISchedulers schedulers,
            IUserLoader userLoader,
            IAvatarLoader avatarLoader)
        {
            return userLoader
                .GetMe()
                .SelectMany(user => avatarLoader.LoadAvatar(user.UserData))
                .SubscribeOn(schedulers.Pool)
                .ObserveOn(schedulers.Main)
                .Subscribe(avatar =>
                {
                    Model.Avatar = avatar;
                });
        }
    }
}