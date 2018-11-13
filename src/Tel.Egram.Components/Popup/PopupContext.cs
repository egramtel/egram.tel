using PropertyChanged;

namespace Tel.Egram.Components.Popup
{
    [AddINotifyPropertyChangedInterface]
    public class PopupContext
    {
        public string PopupTitle { get; set; }
    }
}