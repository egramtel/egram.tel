using System;
using System.Reactive.Linq;
using ReactiveUI;
using Splat;
using Tel.Egram.Graphics;
using Tel.Egram.Messaging.Users;

namespace Tel.Egram.Components.Workspace.Navigation
{
    public static class UserAvatarLogic
    {
        public static IDisposable BindUserAvatar(
            this NavigationModel model)
        {
            return BindUserAvatar(
                model,
                Locator.Current.GetService<IAvatarLoader>(),
                Locator.Current.GetService<IUserLoader>());
        }

        public static IDisposable BindUserAvatar(
            this NavigationModel model,
            IAvatarLoader avatarLoader,
            IUserLoader userLoader)
        {
            return userLoader
                .GetMe()
                .SelectMany(user => avatarLoader.LoadAvatar(user.UserData))
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(avatar =>
                {
                    model.Avatar = avatar;
                });
        }
    }
}