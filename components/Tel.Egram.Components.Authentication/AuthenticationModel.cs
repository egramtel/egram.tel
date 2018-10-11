using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using PropertyChanged;
using ReactiveUI;
using Tel.Egram.Authentication;
using TdLib;
using Tel.Egram.Components.Popup;
using Tel.Egram.Components.Settings.Connection;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Authentication
{
    [AddINotifyPropertyChangedInterface]
    public class AuthenticationModel : IDisposable
    {
        private readonly CompositeDisposable _modelDisposable;

        public ReactiveCommand<Unit, Unit> CheckPasswordCommand { get; }
        public ReactiveCommand<Unit, Unit> CheckCodeCommand { get; }
        public ReactiveCommand<Unit, Unit> SendCodeCommand { get; }
        public ReactiveCommand<Unit, Unit> SetProxyCommand { get; }
        
        public int PasswordIndex { get; set; }
        public int ConfirmIndex { get; set; }
        
        public string PhoneNumber { get; set; }
        public string ConfirmCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }

        public AuthenticationModel(
            IFactory<ProxyPopupModel> proxyPopupModelFactory,
            IAuthenticator authenticator,
            IPopupController popupController
            )
        {
            _modelDisposable = new CompositeDisposable();

            SetProxyCommand = ReactiveCommand.Create(() =>
            {
                var popupModel = proxyPopupModelFactory.Create();
                popupController.Show(popupModel);
            });
            
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
                .DisposeWith(_modelDisposable);

            stateObservable
                .OfType<TdApi.AuthorizationState.AuthorizationStateWaitCode>()
                .Subscribe(state => OnWaitingConfirmCode(!state.IsRegistered))
                .DisposeWith(_modelDisposable);

            stateObservable
                .OfType<TdApi.AuthorizationState.AuthorizationStateWaitPassword>()
                .Subscribe(state => OnWaitingPassword())
                .DisposeWith(_modelDisposable);
        }

        public void Dispose() => _modelDisposable.Dispose();

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