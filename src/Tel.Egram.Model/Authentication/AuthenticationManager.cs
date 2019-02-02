using System;
using System.Reactive.Linq;
using ReactiveUI;
using Splat;
using TdLib;
using Tel.Egram.Model.Authentication.Results;
using Tel.Egram.Services.Authentication;
using Tel.Egram.Services.Utils.Reactive;

namespace Tel.Egram.Model.Authentication
{
    public class AuthenticationManager
    {
        private readonly IAuthenticator _authenticator;

        public AuthenticationManager(
            IAuthenticator authenticator)
        {
            _authenticator = authenticator;
        }

        public AuthenticationManager()
            : this (
                Locator.Current.GetService<IAuthenticator>())
        {
        }
        
        public IDisposable Bind(AuthenticationModel model)
        {
            var canSendCode = model
                .WhenAnyValue(x => x.PhoneNumber)
                .Select(phone => !string.IsNullOrWhiteSpace(phone));
            
            var canCheckCode = model
                .WhenAnyValue(x => x.ConfirmCode)
                .Select(code => !string.IsNullOrWhiteSpace(code));
            
            var canCheckPassword = model
                .WhenAnyValue(x => x.Password)
                .Select(password => !string.IsNullOrWhiteSpace(password));
            
            model.SendCodeCommand = ReactiveCommand.CreateFromObservable(
                (AuthenticationModel m) => SendCode(m.PhoneCode.Code + m.PhoneNumber),
                canSendCode,
                RxApp.MainThreadScheduler);

            model.CheckCodeCommand = ReactiveCommand.CreateFromObservable(
                (AuthenticationModel m) => CheckCode(m.ConfirmCode, m.FirstName, m.LastName),
                canCheckCode,
                RxApp.MainThreadScheduler);
            
            model.CheckPasswordCommand = ReactiveCommand.CreateFromObservable(
                (AuthenticationModel m) => CheckPassword(m.Password),
                canCheckPassword,
                RxApp.MainThreadScheduler);

            return _authenticator.ObserveState()
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Accept(state => HandleState(model, state));
        }

        private void HandleState(AuthenticationModel model, TdApi.AuthorizationState state)
        {
            switch (state)
            {
                case TdApi.AuthorizationState.AuthorizationStateWaitPhoneNumber _:
                    OnWaitingPhoneNumber(model);
                    break;
                        
                case TdApi.AuthorizationState.AuthorizationStateWaitCode wait:
                    OnWaitingConfirmCode(model, !wait.IsRegistered);
                    break;
                        
                case TdApi.AuthorizationState.AuthorizationStateWaitPassword _:
                    OnWaitingPassword(model);
                    break;
            }
        }

        private void OnWaitingPhoneNumber(AuthenticationModel model)
        {
            model.ConfirmIndex = 0;
            model.PasswordIndex = 0;
        }

        private void OnWaitingConfirmCode(AuthenticationModel model, bool isRegistration)
        {
            model.IsRegistration = isRegistration;
            
            model.ConfirmIndex = 1;
            model.PasswordIndex = 0;
        }

        private void OnWaitingPassword(AuthenticationModel model)
        {
            model.ConfirmIndex = 1;
            model.PasswordIndex = 1;
        }

        private IObservable<SendCodeResult> SendCode(string phoneNumber)
        {
            return _authenticator
                .SetPhoneNumber(phoneNumber)
                .Select(_ => new SendCodeResult());
        }

        private IObservable<CheckCodeResult> CheckCode(
            string code,
            string firstName,
            string lastName)
        {
            return _authenticator
                .CheckCode(code, firstName, lastName)
                .Select(_ => new CheckCodeResult());
        }

        private IObservable<CheckPasswordResult> CheckPassword(string password)
        {
            return _authenticator
                .CheckPassword(password)
                .Select(_ => new CheckPasswordResult());
        }
    }
}