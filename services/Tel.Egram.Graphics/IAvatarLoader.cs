using System;
using Avalonia.Media.Imaging;
using TdLib;

namespace Tel.Egram.Graphics
{
    public interface IAvatarLoader
    {
        Avatar GetAvatar(TdApi.User user, AvatarSize size, bool forceFallback = false);
        Avatar GetAvatar(TdApi.Chat chat, AvatarSize size, bool forceFallback = false);
        IObservable<Avatar> LoadAvatar(TdApi.User user, AvatarSize size);
        IObservable<Avatar> LoadAvatar(TdApi.Chat chat, AvatarSize size);
    }
}