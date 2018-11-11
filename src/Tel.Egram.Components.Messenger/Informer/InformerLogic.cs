using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using Tel.Egram.Graphics;
using Tel.Egram.Messaging.Chats;

namespace Tel.Egram.Components.Messenger.Informer
{
    public static class InformerLogic
    {
        public static IDisposable BindInformer(
            this InformerModel model,
            Target target,
            IAvatarLoader avatarLoader = null)
        {
            switch (target)
            {
                case Chat chat:
                    model.IsVisible = true;
                    model.Title = chat.ChatData.Title;
                    model.Label = chat.ChatData.Title;
                    
                    return avatarLoader.LoadAvatar(chat.ChatData)
                        .SubscribeOn(RxApp.TaskpoolScheduler)
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .Subscribe(avatar =>
                        {
                            model.Avatar = avatar;
                        });
                
                case Aggregate aggregate:
                    model.Title = aggregate.Id.ToString();
                    model.Label = aggregate.Id.ToString();
                    
                    return Disposable.Empty;
            }
            
            return Disposable.Empty;
        }
    }
}