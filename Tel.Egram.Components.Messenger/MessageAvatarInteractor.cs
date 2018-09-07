using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using Tel.Egram.Graphics;

namespace Tel.Egram.Components.Messenger
{
    public class MessageAvatarInteractor
    {
        private readonly IAvatarLoader _avatarLoader;

        public MessageAvatarInteractor(
            IAvatarLoader avatarLoader
            )
        {
            _avatarLoader = avatarLoader;
        }

        public IDisposable LoadAuthorAvatar(MessageModel messageModel)
        {
            if (messageModel.AuthorAvatar == null)
            {
                messageModel.AuthorAvatar = _avatarLoader.GetBitmap(messageModel.Message.Chat, AvatarSize.Small);
                messageModel.IsFallbackAuthorAvatar = messageModel.AuthorAvatar == null;
            }
            
            return _avatarLoader.LoadBitmap(messageModel.Message.Chat, AvatarSize.Small)
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(AvaloniaScheduler.Instance)
                .Subscribe(bitmap =>
                {
                    messageModel.AuthorAvatar = bitmap;
                    messageModel.IsFallbackAuthorAvatar = messageModel.AuthorAvatar == null;
                });
        }
    }
}