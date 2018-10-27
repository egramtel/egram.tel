using System;
using System.Collections.Generic;
using System.Reactive;
using DynamicData;
using Tel.Egram.Graphics;
using Tel.Egram.Models.Messenger.Explorer.Messages;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Messenger.Explorer
{
    public interface IAvatarManager
    {
        IObservable<Avatar> ReleaseAvatars(IList<MessageModel> models);
        IObservable<Avatar> PreloadAvatars(IList<MessageModel> models);
        IObservable<Avatar> LoadAvatars(IList<MessageModel> models);
    }
}