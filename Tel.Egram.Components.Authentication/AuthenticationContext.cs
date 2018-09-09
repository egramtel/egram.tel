using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using PropertyChanged;
using ReactiveUI;
using Tel.Egram.Authentication;
using TdLib;

namespace Tel.Egram.Components.Authentication
{
    [AddINotifyPropertyChangedInterface]
    public class AuthenticationContext : IDisposable
    {
        private readonly CompositeDisposable _contextDisposable;

        public ReactiveCommand<Unit, Unit> CheckPasswordCommand { get; }
        public ReactiveCommand<Unit, Unit> CheckCodeCommand { get; }
        public ReactiveCommand<Unit, Unit> SendCodeCommand { get; }
        
        public int PasswordIndex { get; set; }
        public int ConfirmIndex { get; set; }
        
        public string PhoneNumber { get; set; }
        public string ConfirmCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }

        public AuthenticationContext(IAuthenticator authenticator)
        {
            _contextDisposable = new CompositeDisposable();

            var canSendCode = this
                .WhenAnyValue(x => x.PhoneNumber)
                .Select(phone => !string.IsNullOrWhiteSpace(phone));
            
            SendCodeCommand = ReactiveCommand.CreateFromObservable(
                () => authenticator.SetPhoneNumber(PhoneNumber),
                canSendCode, RxApp.MainThreadScheduler);

            var canCheckCode = this
                .WhenAnyValue(x => x.ConfirmCode)
                .Select(code => !string.IsNullOrWhiteSpace(code));
            
            CheckCodeCommand = ReactiveCommand.CreateFromObservable(
                () => authenticator.CheckCode(ConfirmCode, FirstName, LastName),
                canCheckCode, RxApp.MainThreadScheduler);

            var canCheckPassword = this
                .WhenAnyValue(x => x.Password)
                .Select(password => !string.IsNullOrWhiteSpace(password));
            
            CheckPasswordCommand = ReactiveCommand.CreateFromObservable(
                () => authenticator.CheckPassword(Password),
                canCheckPassword, RxApp.MainThreadScheduler);
            
            var stateObservable = authenticator
                .ObserveState()
                .ObserveOn(RxApp.MainThreadScheduler);

            stateObservable
                .OfType<TdApi.AuthorizationState.AuthorizationStateWaitPhoneNumber>()
                .Subscribe(state => OnWaitingPhoneNumber())
                .DisposeWith(_contextDisposable);

            stateObservable
                .OfType<TdApi.AuthorizationState.AuthorizationStateWaitCode>()
                .Subscribe(state => OnWaitingConfirmCode(!state.IsRegistered))
                .DisposeWith(_contextDisposable);

            stateObservable
                .OfType<TdApi.AuthorizationState.AuthorizationStateWaitPassword>()
                .Subscribe(state => OnWaitingPassword())
                .DisposeWith(_contextDisposable);
        }

        public void Dispose() => _contextDisposable.Dispose();

        private void OnWaitingPhoneNumber()
        {
            ConfirmIndex = 0;
            PasswordIndex = 0;
        }

        private void OnWaitingConfirmCode(bool registration)
        {
            ConfirmIndex = 1;
            PasswordIndex = 0;
        }

        private void OnWaitingPassword()
        {
            ConfirmIndex = 1;
            PasswordIndex = 1;
        }
    }
}