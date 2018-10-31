using System;
using Avalonia.Media.Imaging;
using TdLib;

namespace Tel.Egram.Graphics
{
    public interface IAvatarLoader
    {
        Avatar GetAvatar(TdApi.User user, bool forceFallback = false);
        Avatar GetAvatar(TdApi.Chat chat, bool forceFallback = false);
        IObservable<Avatar> LoadAvatar(TdApi.User user);
        IObservable<Avatar> LoadAvatar(TdApi.Chat chat);
    }
}