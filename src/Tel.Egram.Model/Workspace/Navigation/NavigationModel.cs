using System.Reactive.Disposables;
using PropertyChanged;
using ReactiveUI;
using Tel.Egram.Services.Graphics.Avatars;

namespace Tel.Egram.Model.Workspace.Navigation
{
    [AddINotifyPropertyChangedInterface]
    public class NavigationModel : ISupportsActivation
    {
        public Avatar Avatar { get; set; }

        public int SelectedTabIndex { get; set; }

        public NavigationModel()
        {
            this.WhenActivated(disposables =>
            {
                this.BindUserAvatar()
                    .DisposeWith(disposables);
            });
        }
        
        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}