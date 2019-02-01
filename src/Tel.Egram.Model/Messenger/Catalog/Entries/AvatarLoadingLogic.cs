using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Splat;
using TdLib;
using Tel.Egram.Services.Graphics.Avatars;
using Tel.Egram.Services.Utils.Reactive;

namespace Tel.Egram.Model.Messenger.Catalog.Entries
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
                        .Accept(avatar =>
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
                case HomeEntryModel _:
                    return avatarLoader.GetAvatar(
                        AvatarKind.Home,
                        AvatarSize.Small);
                
                case ChatEntryModel chatEntryModel:
                    return avatarLoader.GetAvatar(chatEntryModel.Chat.ChatData, AvatarSize.Small);
                
                case AggregateEntryModel aggregateEntryModel:
                    return avatarLoader.GetAvatar(new TdApi.Chat
                        {
                            Id = aggregateEntryModel.Aggregate.Id
                        },
                        AvatarSize.Small);
            }

            return null;
        }
        
        private static IObservable<Avatar> LoadAvatar(IAvatarLoader avatarLoader, EntryModel entry)
        {   
            switch (entry)
            {
                case HomeEntryModel _:
                    return avatarLoader.LoadAvatar(
                        AvatarKind.Home,
                        AvatarSize.Small);
                
                case ChatEntryModel chatEntryModel:
                    return avatarLoader.LoadAvatar(chatEntryModel.Chat.ChatData, AvatarSize.Small);
                
                case AggregateEntryModel aggregateEntryModel:
                    return avatarLoader.LoadAvatar(new TdApi.Chat
                        {
                            Id = aggregateEntryModel.Aggregate.Id
                        },
                        AvatarSize.Small);
            }
            
            return Observable.Empty<Avatar>();
        }
    }
}