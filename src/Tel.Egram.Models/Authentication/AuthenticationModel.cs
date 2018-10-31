using System.Reactive;
using PropertyChanged;
using ReactiveUI;

namespace Tel.Egram.Models.Authentication
{
    [AddINotifyPropertyChangedInterface]
    public class AuthenticationModel
    {
        public ReactiveCommand<Unit, Unit> CheckPasswordCommand { get; set; }
        public ReactiveCommand<Unit, Unit> CheckCodeCommand { get; set; }
        public ReactiveCommand<Unit, Unit> SendCodeCommand { get; set; }
        public ReactiveCommand<Unit, Unit> SetProxyCommand { get; set; }
        
        public int PasswordIndex { get; set; }
        public int ConfirmIndex { get; set; }
        
        public string PhoneNumber { get; set; }
        public string ConfirmCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
    }
}