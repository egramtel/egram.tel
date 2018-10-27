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
    public class InformerController
        : BaseController, IInformerController
    {
        public InformerController(
            InformerControlModel model,
            IAvatarLoader avatarLoader)
        {
            BindInfo(model, avatarLoader)
                .DisposeWith(this);
        }
        
        private IDisposable BindInfo(
            InformerControlModel model, IAvatarLoader avatarLoader)
        {
            var target = model.Target;
            
            switch (target)
            {
                case Chat chat:
                    model.Title = chat.ChatData.Title;
                    model.Label = chat.ChatData.Title;
                    return avatarLoader.LoadAvatar(chat.ChatData)
                        .SubscribeOn(TaskPoolScheduler.Default)
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