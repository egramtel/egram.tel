using System.Reactive;
using PropertyChanged;
using ReactiveUI;
using Tel.Egram.Messaging.Chats;

namespace Tel.Egram.Models.Messenger.Editor
{
    [AddINotifyPropertyChangedInterface]
    public class EditorModel
    {
        public bool IsVisible { get; set; } = true;
        
        public string Text { get; set; }
        
        public ReactiveCommand<Unit, Unit> SendCommand { get; set; }
        
        public static EditorModel Hidden()
        {
            return new EditorModel
            {
                IsVisible = false
            };
        }
    }
}