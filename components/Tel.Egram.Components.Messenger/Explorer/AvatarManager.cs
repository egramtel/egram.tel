using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;
using Tel.Egram.Graphics;
using Tel.Egram.Models.Messenger.Explorer.Messages;
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

        public IObservable<Avatar> ReleaseAvatars(IList<MessageModel> models)
        {
            return models.ToObservable()
                .Select<MessageModel, Avatar>(messageModel => null);
        }

        public IObservable<Avatar> PreloadAvatars(IList<MessageModel> models)
        {
            return models.ToObservable()
                .Select(messageModel =>
                {
                    var user = messageModel.Message.User;
                    var chat = messageModel.Message.Chat;

                    return user == null
                        ? _avatarLoader.GetAvatar(chat)
                        : _avatarLoader.GetAvatar(user);
                });
        }

        public IObservable<Avatar> LoadAvatars(IList<MessageModel> models)
        {
            return models.ToObservable()
                .SelectMany(messageModel =>
                {
                    var user = messageModel.Message.User;
                    var chat = messageModel.Message.Chat;

                    if (messageModel.Avatar?.Bitmap == null)
                    {
                        return user == null
                            ? _avatarLoader.LoadAvatar(chat)
                            : _avatarLoader.LoadAvatar(user);
                    }

                    return Observable.Return<Avatar>(null);
                });
        }
    }
}