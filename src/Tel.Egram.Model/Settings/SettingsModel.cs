using PropertyChanged;
using ReactiveUI;

namespace Tel.Egram.Model.Settings
{
    [AddINotifyPropertyChangedInterface]
    public class SettingsModel : ISupportsActivation
    {
        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}