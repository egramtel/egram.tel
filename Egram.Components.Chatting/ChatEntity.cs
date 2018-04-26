using System;
using ReactiveUI;

namespace Egram.Components.Chatting
{
    public abstract class ChatEntity : ReactiveObject
    {
        public abstract bool IsDateBadge { get; }
        
        public abstract bool IsTextMessage { get; }
    }
}