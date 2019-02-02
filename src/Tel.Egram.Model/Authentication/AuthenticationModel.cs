using System.Reactive;
using System.Reactive.Disposables;
using DynamicData.Binding;
using PropertyChanged;
using ReactiveUI;
using Tel.Egram.Model.Authentication.Phone;
using Tel.Egram.Model.Authentication.Proxy;
using Tel.Egram.Model.Authentication.Results;

namespace Tel.Egram.Model.Authentication
{
    [AddINotifyPropertyChangedInterface]
    public class AuthenticationModel : ISupportsActivation
    {
        public ReactiveCommand<Unit, Unit> SetProxyCommand { get; set; }
        
        public ReactiveCommand<AuthenticationModel, SendCodeResult> SendCodeCommand { get; set; }
        public ReactiveCommand<AuthenticationModel, CheckCodeResult> CheckCodeCommand { get; set; }
        public ReactiveCommand<AuthenticationModel, CheckPasswordResult> CheckPasswordCommand { get; set; }
        
        public bool IsRegistration { get; set; }
        
        public int PasswordIndex { get; set; }
        public int ConfirmIndex { get; set; }
        
        public ObservableCollectionExtended<PhoneCodeModel> PhoneCodes { get; set; }
        public PhoneCodeModel PhoneCode { get; set; }
        
        public string PhoneNumber { get; set; }
        public int PhoneNumberStart { get; set; }
        public int PhoneNumberEnd { get; set; }
        
        public string ConfirmCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }

        public AuthenticationModel()
        {
            this.WhenActivated(disposables =>
            {
                new PhoneCodeLoader()
                    .Bind(this)
                    .DisposeWith(disposables);

                new AuthenticationManager()
                    .Bind(this)
                    .DisposeWith(disposables);

                new ProxyManager()
                    .Bind(this)
                    .DisposeWith(disposables);
            });
        }

        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}