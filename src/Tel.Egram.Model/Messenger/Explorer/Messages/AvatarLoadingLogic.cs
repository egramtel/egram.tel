using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using Splat;
using Tel.Egram.Services.Graphics.Avatars;
using Tel.Egram.Services.Utils.Reactive;

namespace Tel.Egram.Model.Messenger.Explorer.Messages
{
    public static class AvatarLoadingLogic
    {
        public static IDisposable BindAvatarLoading(
            this MessageModel model)
        {
            return BindAvatarLoading(
                model,
                Locator.Current.GetService<IAvatarLoader>());
        }
        
        public static IDisposable BindAvatarLoading(
            this MessageModel model,
            IAvatarLoader avatarLoader)
        {
            if (model.Avatar == null)
            {
                model.Avatar = GetAvatar(avatarLoader, model);

                if (model.Avatar == null || model.Avatar.IsFallback)
                {
                    return LoadAvatar(avatarLoader, model)
                        .SubscribeOn(RxApp.TaskpoolScheduler)
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .Accept(avatar =>
                        {
                            model.Avatar = avatar;
                        });
                }
            }
            
            return Disposable.Empty;
        }

        private static Avatar GetAvatar(IAvatarLoader avatarLoader, MessageModel entry)
        {
            if (entry.Message?.UserData != null)
            {
                return avatarLoader.GetAvatar(entry.Message.UserData, AvatarSize.Regular);
            }

            if (entry.Message?.ChatData != null)
            {
                return avatarLoader.GetAvatar(entry.Message.ChatData, AvatarSize.Regular);
            }
            
            return null;
        }
        
        private static IObservable<Avatar> LoadAvatar(IAvatarLoader avatarLoader, MessageModel entry)
        {
            if (entry.Message?.UserData != null)
            {
                return avatarLoader.LoadAvatar(entry.Message.UserData, AvatarSize.Regular);
            }

            if (entry.Message?.ChatData != null)
            {
                return avatarLoader.LoadAvatar(entry.Message.ChatData, AvatarSize.Regular);
            }
            
            return Observable.Empty<Avatar>();
        }
    }
}