using System;
using Avalonia.Media.Imaging;
using TdLib;

namespace Tel.Egram.Graphics
{
    public interface IAvatarLoader
    {
        Avatar GetAvatar(TdApi.User user, AvatarSize size);
        Avatar GetAvatar(TdApi.Chat chat, AvatarSize size);
        IObservable<Avatar> LoadAvatar(TdApi.User user, AvatarSize size);
        IObservable<Avatar> LoadAvatar(TdApi.Chat chat, AvatarSize size);
    }
}