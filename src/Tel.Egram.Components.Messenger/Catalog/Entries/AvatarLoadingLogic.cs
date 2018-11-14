using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Splat;
using TdLib;
using Tel.Egram.Graphics;
using Tel.Egram.Messaging.Chats;

namespace Tel.Egram.Components.Messenger.Catalog.Entries
{
    public static class AvatarLoadingLogic
    {
        public static IDisposable BindAvatarLoading(
            this EntryModel model)
        {
            return BindAvatarLoading(
                model,
                Locator.Current.GetService<IAvatarLoader>());
        }
        
        public static IDisposable BindAvatarLoading(
            this EntryModel model,
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

        private static Avatar GetAvatar(IAvatarLoader avatarLoader, EntryModel entry)
        {
            switch (entry.Target)
            {
                case Chat chat:
                    return avatarLoader.GetAvatar(chat.ChatData);
                
                case Aggregate aggregate:
                    return avatarLoader.GetAvatar(new TdApi.Chat
                    {
                        Id = aggregate.Id
                    });
            }

            return null;
        }
        
        private static IObservable<Avatar> LoadAvatar(IAvatarLoader avatarLoader, EntryModel entry)
        {   
            switch (entry.Target)
            {
                case Chat chat:
                    return avatarLoader.LoadAvatar(chat.ChatData);
                
                case Aggregate aggregate:
                    return avatarLoader.LoadAvatar(new TdApi.Chat
                    {
                        Id = aggregate.Id
                    });
            }
            
            return Observable.Empty<Avatar>();
        }
    }
}