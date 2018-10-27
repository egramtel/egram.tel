using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using ReactiveUI;
using TdLib;
using Tel.Egram.Gui.Views.Messenger.Editor;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Messaging.Messages;

namespace Tel.Egram.Components.Messenger.Editor
{
    public class EditorController
        : BaseController, IEditorController
    {
        public EditorController(
            EditorControlModel model,
            IMessageSender messageSender)
        {
            switch (model.Target)
            {
                case Chat chat:
                    BindMessageSender(model, chat, messageSender)
                        .DisposeWith(this);
                    break;
            }
        }
        
        private IDisposable BindMessageSender(
            EditorControlModel model,
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
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ =>
                {
                    model.Text = null;
                });
        }
    }
}