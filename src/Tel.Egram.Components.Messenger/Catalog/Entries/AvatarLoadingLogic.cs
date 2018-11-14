using System;
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
            return LoadAvatar(avatarLoader, model)
                .Subscribe(avatar =>
                {
                    model.Avatar = avatar;
                });
        }
        
        private static IObservable<Avatar> LoadAvatar(IAvatarLoader avatarLoader, EntryModel entry)
        {
            if (entry.Avatar != null)
            {
                return Observable.Return(entry.Avatar);
            }
            
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
            
            return Observable.Return<Avatar>(null);
        }
    }
}