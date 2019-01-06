using System.Reactive.Disposables;
using PropertyChanged;
using ReactiveUI;

namespace Tel.Egram.Model.Popups
{
    [AddINotifyPropertyChangedInterface]
    public class PopupModel : ISupportsActivation
    {
        public PopupContext[] Contexts { get; set; }
        
        public PopupContext Context { get; set; }

        public bool IsVisible { get; set; } = true;

        public PopupModel(PopupContext context)
        {
            Contexts = new[] { context };
            Context = context;
            
            this.WhenActivated(disposables =>
            {
                this.BindPopup()
                    .DisposeWith(disposables);
            });
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