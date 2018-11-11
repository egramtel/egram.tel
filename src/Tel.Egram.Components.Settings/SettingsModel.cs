using PropertyChanged;
using ReactiveUI;

namespace Tel.Egram.Components.Settings
{
    [AddINotifyPropertyChangedInterface]
    public class SettingsModel : ISupportsActivation
    {
        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}