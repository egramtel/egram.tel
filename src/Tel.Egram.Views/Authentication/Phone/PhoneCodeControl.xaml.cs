using Avalonia.Markup.Xaml;
using Tel.Egram.Model.Authentication;
using Tel.Egram.Model.Authentication.Phone;

namespace Tel.Egram.Views.Authentication.Phone
{
    public class PhoneCodeControl : BaseControl<PhoneCodeModel>
    {
        public PhoneCodeControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
