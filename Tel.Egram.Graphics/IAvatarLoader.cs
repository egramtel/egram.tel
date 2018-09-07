using System;
using Avalonia.Media.Imaging;
using TdLib;

namespace Tel.Egram.Graphics
{
    public interface IAvatarLoader
    {
        IBitmap GetBitmap(TdApi.User user, AvatarSize size);
        IBitmap GetBitmap(TdApi.Chat chat, AvatarSize size);
        IObservable<IBitmap> LoadBitmap(TdApi.User user, AvatarSize size);
        IObservable<IBitmap> LoadBitmap(TdApi.Chat chat, AvatarSize size);
    }
}