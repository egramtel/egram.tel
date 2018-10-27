using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using Tel.Egram.Graphics;
using Tel.Egram.Gui.Views.Messenger.Informer;
using Tel.Egram.Messaging.Chats;

namespace Tel.Egram.Components.Messenger.Informer
{
    public class InformerController : BaseController<InformerControlModel>
    {
        public InformerController(
            Target target,
            IAvatarLoader avatarLoader)
        {
            BindInfo(target, avatarLoader)
                .DisposeWith(this);
        }
        
        private IDisposable BindInfo(
            Target target, IAvatarLoader avatarLoader)
        {   
            switch (target)
            {
                case Chat chat:
                    Model.Title = chat.ChatData.Title;
                    Model.Label = chat.ChatData.Title;
                    return avatarLoader.LoadAvatar(chat.ChatData)
                        .SubscribeOn(TaskPoolScheduler.Default)
                        .ObserveOn(RxApp.MainThreadScheduler)
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