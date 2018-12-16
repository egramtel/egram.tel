using System;
using System.Reactive;
using System.Reactive.Disposables;
using PropertyChanged;
using ReactiveUI;
using TdLib;
using Tel.Egram.Components.Authentication.Results;

namespace Tel.Egram.Components.Authentication
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
        
        public string PhoneNumber { get; set; }
        public string ConfirmCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }

        public AuthenticationModel()
        {
            this.WhenActivated(disposables =>
            {
                this.BindAuthentication()
                    .DisposeWith(disposables);

                this.BindProxySettings()
                    .DisposeWith(disposables);
            });
        }

        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}