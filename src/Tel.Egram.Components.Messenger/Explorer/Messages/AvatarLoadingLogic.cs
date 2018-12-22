using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Splat;
using TdLib;
using Tel.Egram.Graphics;

namespace Tel.Egram.Components.Messenger.Explorer.Messages
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
                        .Subscribe(avatar =>
                        {
                            model.Avatar = avatar;
                        });
                }
            }
            
            return Disposable.Empty;
        }

        private static Avatar GetAvatar(IAvatarLoader avatarLoader, MessageModel entry)
        {
            if (entry.Message?.User != null)
            {
                return avatarLoader.GetAvatar(entry.Message.User);
            }
            
            return null;
        }
        
        private static IObservable<Avatar> LoadAvatar(IAvatarLoader avatarLoader, MessageModel entry)
        {
            if (entry.Message?.User != null)
            {
                return avatarLoader.LoadAvatar(entry.Message.User);
            }
            
            return Observable.Empty<Avatar>();
        }
    }
}