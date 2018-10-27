using System.Reactive;
using PropertyChanged;
using ReactiveUI;
using TdLib;
using Tel.Egram.Messaging.Chats;

namespace Tel.Egram.Gui.Views.Messenger.Editor
{
    [AddINotifyPropertyChangedInterface]
    public class EditorControlModel
    {
        public bool IsVisible { get; set; }
        
        public string Text { get; set; }
        
        public ReactiveCommand<Unit, Unit> SendCommand { get; set; }
        
        public Target Target { get; set; }
        
        public static EditorControlModel Hidden()
        {
            return new EditorControlModel();
        }

        public static EditorControlModel FromTarget(Target target)
        {
            return new EditorControlModel
            {
                IsVisible = true,
                Target = target
            };
        }
    }
}