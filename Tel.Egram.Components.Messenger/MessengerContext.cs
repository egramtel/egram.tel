using System;
using System.Reactive.Disposables;
using PropertyChanged;
using ReactiveUI;
using Tel.Egram.Components.MessageEditor;

namespace Tel.Egram.Components.Messenger
{
    [AddINotifyPropertyChangedInterface]
    public abstract class MessengerContext : IDisposable
    {   
        protected CompositeDisposable _contextDisposable = new CompositeDisposable();
        
        public ReactiveList<MessageModel> Messages { get; set; } = new ReactiveList<MessageModel>();
        
        public MessageEditorContext MessageEditorContext { get; set; }

        public bool IsMessageEditorVisible { get; set; }

        public virtual void Dispose()
        {
            MessageEditorContext?.Dispose();
            
            _contextDisposable.Dispose();
        }
    }
}