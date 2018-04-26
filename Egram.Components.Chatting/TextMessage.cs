using System;
using ReactiveUI;

namespace Egram.Components.Chatting
{
    public class TextMessage : BaseMessage
    {
        public string _text;
        public string Text
        {
            get => _text;
            set => this.RaiseAndSetIfChanged(ref _text, value);
        }

        public override bool IsDateBadge => false;

        public override bool IsTextMessage => true;
    }
}