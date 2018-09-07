using Avalonia.Media.Imaging;
using ReactiveUI;

namespace Tel.Egram.Components.Navigation
{
    public class NavigationTab : ReactiveObject
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }
        
        private IBitmap _icon;
        public IBitmap Icon
        {
            get => _icon;
            set => this.RaiseAndSetIfChanged(ref _icon, value);
        }
    }
}