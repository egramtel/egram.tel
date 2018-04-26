using System;
using Avalonia.Media.Imaging;
using ReactiveUI;

namespace Egram.Components.Chatting
{
    public abstract class BaseMessage : ChatEntity
    {
        private long _id;
        public long Id
        {
            get => _id;
            set => this.RaiseAndSetIfChanged(ref _id, value);
        }
        
        private DateTimeOffset _dateTime;
        public DateTimeOffset DateTime
        {
            get => _dateTime;
            set => this.RaiseAndSetIfChanged(ref _dateTime, value);
        }

        public string Time => DateTime.ToString("HH:mm");

        private bool _isOutgoing;
        public bool IsOutgoing
        {
            get => _isOutgoing;
            set => this.RaiseAndSetIfChanged(ref _isOutgoing, value);
        }

        private bool _isIncoming;
        public bool IsIncoming
        {
            get => _isIncoming;
            set => this.RaiseAndSetIfChanged(ref _isIncoming, value);
        }

        private bool _isAddition;
        public bool IsAddition
        {
            get => _isAddition;
            set => this.RaiseAndSetIfChanged(ref _isAddition, value);
        }

        private bool _hasAddition;
        public bool HasAddition
        {
            get => _hasAddition;
            set => this.RaiseAndSetIfChanged(ref _hasAddition, value);
        }

        public bool HasAvatar => !IsAddition;

        private MessageAuthor _messageAuthor;
        public MessageAuthor MessageAuthor
        {
            get => _messageAuthor;
            set => this.RaiseAndSetIfChanged(ref _messageAuthor, value);
        }
    }
}