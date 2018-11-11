using System;
using System.Reactive.Linq;
using ReactiveUI;
using Tel.Egram.Graphics;
using Tel.Egram.Messaging.Users;

namespace Tel.Egram.Components.Workspace.Navigation
{
    public static class UserAvatarLogic
    {
        public static IDisposable BindUserAvatar(
            this NavigationModel model,
            IAvatarLoader avatarLoader = null,
            IUserLoader userLoader = null)
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