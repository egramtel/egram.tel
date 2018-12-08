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
            switch (entry)
            {
                case ChatEntryModel chatEntryModel:
                    return avatarLoader.GetAvatar(chatEntryModel.Chat.ChatData);
                
                case AggregateEntryModel aggregateEntryModel:
                    return avatarLoader.GetAvatar(new TdApi.Chat
                    {
                        Id = aggregateEntryModel.Aggregate.Id
                    });
            }

            return null;
        }
        
        private static IObservable<Avatar> LoadAvatar(IAvatarLoader avatarLoader, EntryModel entry)
        {   
            switch (entry)
            {
                case ChatEntryModel chatEntryModel:
                    return avatarLoader.LoadAvatar(chatEntryModel.Chat.ChatData);
                
                case AggregateEntryModel aggregateEntryModel:
                    return avatarLoader.LoadAvatar(new TdApi.Chat
                    {
                        Id = aggregateEntryModel.Aggregate.Id
                    });
            }
            
            return Observable.Empty<Avatar>();
        }
    }
}