using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Tel.Egram.Components.MessageEditor;
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

        private bool _isMessageEditorVisible;
        public bool IsMessageEditorVisible
        {
            get => _isMessageEditorVisible;
            set => this.RaiseAndSetIfChanged(ref _isMessageEditorVisible, value);
        }
        
        private MessageEditorContext _messageEditorContext;
        public MessageEditorContext MessageEditorContext
        {
            get => _messageEditorContext;
            set => this.RaiseAndSetIfChanged(ref _messageEditorContext, value);
        }

        public virtual void Dispose()
        {
            MessageEditorContext?.Dispose();
        }
    }
}