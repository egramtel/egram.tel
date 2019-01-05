using System;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using Splat;
using TdLib;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Messaging.Messages;
using Tel.Egram.Utils.Reactive;

namespace Tel.Egram.Components.Messenger.Editor
{
    public static class SenderLogic
    {
        public static IDisposable BindSender(
            this EditorModel model,
            Chat chat)
        {
            return BindSender(
                model,
                chat,
                Locator.Current.GetService<IMessageSender>());
        }

        public static IDisposable BindSender(
            this EditorModel model,
            Chat chat,
            IMessageSender messageSender)
        {
            var canSendCode = model
                .WhenAnyValue(m => m.Text)
                .Select(text => !string.IsNullOrWhiteSpace(text));
            
            model.SendCommand = ReactiveCommand.CreateFromObservable(
                () => messageSender.SendMessage(chat.ChatData,
                        new TdApi.InputMessageContent.InputMessageText
                        {
                            ClearDraft = true,
                            Text = new TdApi.FormattedText
                            {
                                Text = model.Text
                            }
                        })
                    .Select(_ => Unit.Default),
                canSendCode,
                RxApp.MainThreadScheduler);

            return model.SendCommand
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Accept(_ =>
                {
                    model.Text = null;
                });
        }
    }
}