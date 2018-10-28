using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Avalonia.Threading;
using ReactiveUI;
using Tel.Egram.Graphics;
using Tel.Egram.Messaging.Users;
using Tel.Egram.Models.Workspace.Navigation;

namespace Tel.Egram.Components.Workspace.Navigation
{
    public class NavigationController : BaseController<NavigationModel>
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
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(avatar =>
                {
                    Model.Avatar = avatar;
                });
        }
    }
}