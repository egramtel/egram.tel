using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using PropertyChanged;
using ReactiveUI;
using TdLib;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Messaging.Messages;

namespace Tel.Egram.Components.Messenger.Editor
{
    [AddINotifyPropertyChangedInterface]
    public class EditorModel : IDisposable
    {
        private readonly CompositeDisposable _modelDisposable = new CompositeDisposable();
        
        public string Text { get; set; }
        
        public ReactiveCommand<Unit, TdApi.Message> SendCommand { get; set; }
        
        public EditorModel(
            Target target,
            IMessageSender messageSender
            )
        {
            switch (target)
            {
                case Chat chat:
                    BindMessageSender(chat, messageSender)
                        .DisposeWith(_modelDisposable);
                    break;
            }
        }

        private IDisposable BindMessageSender(Chat chat, IMessageSender messageSender)
        {
            var canSendCode = this
                .WhenAnyValue(x => x.Text)
                .Select(text => !string.IsNullOrWhiteSpace(text));
            
            SendCommand = ReactiveCommand.CreateFromObservable(
                () => messageSender.SendMessage(chat.ChatData,
                    new TdApi.InputMessageContent.InputMessageText
                    {
                        ClearDraft = true,
                        Text = new TdApi.FormattedText
                        {
                            Text = Text
                        }
                    }),
                canSendCode,
                RxApp.MainThreadScheduler);

            return SendCommand
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ =>
                {
                    Text = null;
                });
        }

        public void Dispose()
        {
            _modelDisposable.Dispose();
        }
    }
}