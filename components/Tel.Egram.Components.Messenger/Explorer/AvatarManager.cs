using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;
using Tel.Egram.Components.Messenger.Explorer.Messages;
using Tel.Egram.Graphics;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Messenger.Explorer
{
    public class AvatarManager : IAvatarManager
    {
        private readonly IAvatarLoader _avatarLoader;

        public AvatarManager(
            IAvatarLoader avatarLoader)
        {
            _avatarLoader = avatarLoader;
        }

        public IObservable<Action> ReleaseAvatars(SourceList<ItemModel> items, Range prevRange, Range range)
        {
            var messageModels = new List<MessageModel>();
            
            items.Edit(list =>
            {
                // release prev avatar bitmaps
                for (int i = prevRange.From; i <= prevRange.To; i++)
                {           
                    // do not release items within current range
                    if (i >= range.From && i <= range.To)
                    {
                        continue;
                    }

                    if (i >= list.Count)
                    {
                        break;
                    }
                    
                    var item = list[i];
                    if (item is MessageModel messageModel)
                    {
                        messageModels.Add(messageModel);
                    }
                }
            });
            
            return messageModels.ToObservable()
                .Select(messageModel => new Action(() =>
                    {
                        var user = messageModel.Message.User;
                        var chat = messageModel.Message.Chat;

                        messageModel.Avatar = user == null
                            ? _avatarLoader.GetAvatar(chat, AvatarSize.Big)
                            : _avatarLoader.GetAvatar(user, AvatarSize.Big);
                    }));
        }

        public IObservable<Action> LoadAvatars(SourceList<ItemModel> items, Range prevRange, Range range)
        {
            var messageModels = new List<MessageModel>();
            
            items.Edit(list =>
            {
                // load avatar bitmaps for new range
                for (int i = range.From; i <= range.To; i++)
                {
                    if (i >= list.Count)
                    {
                        break;
                    }
                    
                    var item = list[i];
                    if (item is MessageModel messageModel)
                    {
                        messageModels.Add(messageModel);
                    }
                }
            });

            return messageModels.ToObservable()
                .SelectMany(messageModel =>
                {
                    var user = messageModel.Message.User;
                    var chat = messageModel.Message.Chat;

                    var observable = Observable.Empty<Action>();
                    
                    if (messageModel.Avatar?.Bitmap == null)
                    {
                        var avatar = user == null
                            ? _avatarLoader.GetAvatar(chat, AvatarSize.Big)
                            : _avatarLoader.GetAvatar(user, AvatarSize.Big);
                        
                        observable = observable.Concat(Observable.Return(
                                new Action(() =>
                                {
                                    messageModel.Avatar = avatar;
                                })
                            ));
                    }

                    if (messageModel.Avatar?.Bitmap == null)
                    {
                        if (user == null)
                        {
                            observable = observable.Concat(_avatarLoader.LoadAvatar(chat, AvatarSize.Big)
                                    .Select(avatar => new Action(() =>
                                    {
                                        messageModel.Avatar = avatar;
                                    }))
                                );
                        }
                        else
                        {
                            observable = observable.Concat(_avatarLoader.LoadAvatar(user, AvatarSize.Big)
                                    .Select(avatar => new Action(() =>
                                    {
                                        messageModel.Avatar = avatar;
                                    }))
                                );
                        }
                    }

                    return observable;
                });
        }
    }
}