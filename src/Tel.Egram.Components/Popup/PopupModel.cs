using System.Reactive;
using PropertyChanged;
using ReactiveUI;

namespace Tel.Egram.Components.Popup
{
    [AddINotifyPropertyChangedInterface]
    public class PopupModel : ISupportsActivation
    {
        public PopupContext PopupContext { get; set; }

        public bool IsVisible { get; set; } = true;
        
        public int PopupIndex { get; set; }
        
        public ReactiveCommand<Unit, Unit> PopupCloseCommand { get; }

        public PopupModel(PopupContext context)
        {
            PopupContext = context;
        }

        private PopupModel()
        {
        }

        public static PopupModel Hidden()
        {
            return new PopupModel
            {
                IsVisible = false
            };
        }

        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}