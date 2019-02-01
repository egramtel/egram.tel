using System;
using TdLib;

namespace Tel.Egram.Services.Graphics.Avatars
{
    public interface IAvatarLoader
    {
        Avatar GetAvatar(AvatarKind kind, AvatarSize size);
        Avatar GetAvatar(TdApi.User user, AvatarSize size);
        Avatar GetAvatar(TdApi.Chat chat, AvatarSize size);

        IObservable<Avatar> LoadAvatar(AvatarKind kind, AvatarSize size);
        IObservable<Avatar> LoadAvatar(TdApi.User user, AvatarSize size);
        IObservable<Avatar> LoadAvatar(TdApi.Chat chat, AvatarSize size);
    }
}