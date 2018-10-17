using System;
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


        public IDisposable ReleaseAvatars(SourceList<ItemModel> items, Range prevRange, Range range)
        {
            // release prev avatar bitmaps
            for (int i = prevRange.From; i <= prevRange.To; i++)
            {
                int index = i;
                        
                // do not release items within current range
                if (index >= range.From && index <= range.To)
                {
                    continue;
                }

                items.Edit(list =>
                {
                    var item = list[index];
                    if (item is MessageModel messageModel)
                    {
                        var user = messageModel.Message.User;
                        var chat = messageModel.Message.Chat;
                                
                        messageModel.Avatar = user == null
                            ? _avatarLoader.GetAvatar(chat, AvatarSize.Big)
                            : _avatarLoader.GetAvatar(user, AvatarSize.Big);
                    }
                });
            }
            
            return Disposable.Empty;
        }

        public IDisposable LoadAvatars(SourceList<ItemModel> items, Range prevRange, Range range)
        {
            var disposable = new CompositeDisposable();
            
            // load avatar bitmaps for new range
            for (int i = range.From; i <= range.To; i++)
            {
                int index = i;
                
                items.Edit(list =>
                {
                    var item = list[index];
                    if (item is MessageModel messageModel)
                    {
                        var user = messageModel.Message.User;
                        var chat = messageModel.Message.Chat;
                        
                        if (messageModel.Avatar?.Bitmap == null)
                        {
                            messageModel.Avatar = user == null
                                ? _avatarLoader.GetAvatar(chat, AvatarSize.Big)
                                : _avatarLoader.GetAvatar(user, AvatarSize.Big);
                        }
                        
                        if (messageModel.Avatar?.Bitmap == null)
                        {
                            if (user == null)
                            {
                                _avatarLoader.LoadAvatar(chat, AvatarSize.Big)
                                    .ObserveOn(TaskPoolScheduler.Default)
                                    .SubscribeOn(RxApp.MainThreadScheduler)
                                    .Subscribe(avatar =>
                                    {
                                        messageModel.Avatar = avatar; 
                                    })
                                    .DisposeWith(disposable);
                            }
                            else
                            {
                                _avatarLoader.LoadAvatar(user, AvatarSize.Big)
                                    .ObserveOn(TaskPoolScheduler.Default)
                                    .SubscribeOn(RxApp.MainThreadScheduler)
                                    .Subscribe(avatar =>
                                    {
                                        messageModel.Avatar = avatar; 
                                    })
                                    .DisposeWith(disposable);
                            }
                        }
                    }
                });
            }

            return disposable;
        }
    }
}