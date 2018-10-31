using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using ReactiveUI;
using TdLib;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Messaging.Messages;
using Tel.Egram.Models.Messenger.Editor;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Messenger.Editor
{
    public class EditorController : Controller<EditorModel>
    {
        public EditorController(
            Target target,
            ISchedulers schedulers,
            IMessageSender messageSender)
        {
            switch (target)
            {
                case Chat chat:
                    BindMessageSender(chat, schedulers, messageSender)
                        .DisposeWith(this);
                    break;
            }
        }
        
        private IDisposable BindMessageSender(
            Chat chat,
            ISchedulers schedulers,
            IMessageSender messageSender)
        {
            var canSendCode = Model
                .WhenAnyValue(m => m.Text)
                .Select(text => !string.IsNullOrWhiteSpace(text));
            
            Model.SendCommand = ReactiveCommand.CreateFromObservable(
                () => messageSender.SendMessage(chat.ChatData,
                    new TdApi.InputMessageContent.InputMessageText
                    {
                        ClearDraft = true,
                        Text = new TdApi.FormattedText
                        {
                            Text = Model.Text
                        }
                    })
                    .Select(_ => Unit.Default),
                canSendCode,
                schedulers.Main);

            return Model.SendCommand
                .SubscribeOn(schedulers.Pool)
                .ObserveOn(schedulers.Main)
                .Subscribe(_ =>
                {
                    Model.Text = null;
                });
        }
    }
}