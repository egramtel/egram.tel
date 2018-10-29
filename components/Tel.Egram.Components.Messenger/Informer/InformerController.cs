using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using Tel.Egram.Graphics;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Models.Messenger.Informer;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Messenger.Informer
{
    public class InformerController : Controller<InformerModel>
    {
        public InformerController(
            Target target,
            ISchedulers schedulers,
            IAvatarLoader avatarLoader)
        {
            BindInfo(target, schedulers, avatarLoader)
                .DisposeWith(this);
        }
        
        private IDisposable BindInfo(
            Target target,
            ISchedulers schedulers,
            IAvatarLoader avatarLoader)
        {   
            switch (target)
            {
                case Chat chat:
                    Model.IsVisible = true;
                    Model.Title = chat.ChatData.Title;
                    Model.Label = chat.ChatData.Title;
                    return avatarLoader.LoadAvatar(chat.ChatData)
                        .SubscribeOn(schedulers.Pool)
                        .ObserveOn(schedulers.Main)
                        .Subscribe(avatar =>
                        {
                            Model.Avatar = avatar;
                        });
                
                case Aggregate aggregate:
                    Model.Title = aggregate.Id.ToString();
                    Model.Label = aggregate.Id.ToString();
                    return Disposable.Empty;
            }
            
            return Disposable.Empty;
        }
    }
}