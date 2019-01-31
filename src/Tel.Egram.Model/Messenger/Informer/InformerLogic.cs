using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using Splat;
using Tel.Egram.Services.Graphics.Avatars;
using Tel.Egram.Services.Messaging.Chats;
using Tel.Egram.Services.Utils.Reactive;

namespace Tel.Egram.Model.Messenger.Informer
{
    public static class InformerLogic
    {
        public static IDisposable BindInformer(
            this InformerModel model,
            Chat chat)
        {
            return BindInformer(
                model,
                chat,
                Locator.Current.GetService<IAvatarLoader>());
        }

        public static IDisposable BindInformer(
            this InformerModel model,
            Aggregate aggregate)
        {
            return BindInformer(
                model,
                aggregate,
                Locator.Current.GetService<IAvatarLoader>());
        }

        public static IDisposable BindInformer(
            this InformerModel model,
            Chat chat,
            IAvatarLoader avatarLoader)
        {
            model.Title = chat.ChatData.Title;
            model.Label = chat.ChatData.Title;
                    
            return avatarLoader.LoadAvatar(chat.ChatData, AvatarSize.Regular)
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Accept(avatar =>
                {
                    model.Avatar = avatar;
                });
        }
        
        public static IDisposable BindInformer(
            this InformerModel model,
            Aggregate aggregate,
            IAvatarLoader avatarLoader)
        {
            model.Title = aggregate.Id.ToString();
            model.Label = aggregate.Id.ToString();
            
            return Disposable.Empty;
        }
    }
}