using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using PropertyChanged;
using ReactiveUI;
using Tel.Egram.Graphics;
using Tel.Egram.Messaging.Chats;

namespace Tel.Egram.Components.Messenger.Informer
{
    [AddINotifyPropertyChangedInterface]
    public class InformerModel : IDisposable
    {
        private readonly CompositeDisposable _modelDisposable = new CompositeDisposable();
        
        public string Title { get; set; }
        
        public string Label { get; set; }
        
        public Avatar Avatar { get; set; }

        public InformerModel(
            IAvatarLoader avatarLoader,
            Target target
            )
        {
            BindInfo(avatarLoader, target)
                .DisposeWith(_modelDisposable);
        }

        private IDisposable BindInfo(IAvatarLoader avatarLoader, Target target)
        {
            switch (target)
            {
                case Chat chat:
                    Title = chat.ChatData.Title;
                    Label = chat.ChatData.Title;
                    return avatarLoader.LoadAvatar(chat.ChatData)
                        .SubscribeOn(TaskPoolScheduler.Default)
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .Subscribe(avatar =>
                        {
                            Avatar = avatar;
                        });
                
                case Aggregate aggregate:
                    Title = aggregate.Id.ToString();
                    Label = aggregate.Id.ToString();
                    return Disposable.Empty;
            }
            
            return Disposable.Empty;
        }

        public void Dispose()
        {
            _modelDisposable.Dispose();
        }
    }
}