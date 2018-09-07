using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Tel.Egram.Components.MessageEditor;
using Tel.Egram.Components.Messenger.Aggregate;
using Tel.Egram.Feeds;
using Tel.Egram.Messages;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Messenger
{
    public abstract class MessengerContext : ReactiveObject, IDisposable
    {   
        private ReactiveList<MessageModel> _messages = new ReactiveList<MessageModel>();
        public ReactiveList<MessageModel> Messages
        {
            get => _messages;
            set => this.RaiseAndSetIfChanged(ref _messages, value);
        }

        private MessageEditorContext _messageEditorContext;
        public MessageEditorContext MessageEditorContext
        {
            get => _messageEditorContext;
            set => this.RaiseAndSetIfChanged(ref _messageEditorContext, value);
        }

        public abstract void OnPrependMessages(List<Message> messages);

        public abstract void OnAppendMessages(List<Message> messages);
        
        public abstract void Dispose();
    }
}