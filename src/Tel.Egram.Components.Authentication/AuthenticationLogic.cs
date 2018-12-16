using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using Splat;
using TdLib;
using Tel.Egram.Authentication;
using Tel.Egram.Components.Authentication.Results;

namespace Tel.Egram.Components.Authentication
{
    public static class AuthenticationLogic
    {
        public static IDisposable BindAuthentication(
            this AuthenticationModel model)
        {
            return BindAuthentication(
                model,
                Locator.Current.GetService<IAuthenticator>());
        }

        public static IDisposable BindAuthentication(
            this AuthenticationModel model,
            IAuthenticator authenticator)
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
                (AuthenticationModel m) => SendCode(authenticator, m.PhoneNumber),
                canSendCode,
                RxApp.MainThreadScheduler);

            model.CheckCodeCommand = ReactiveCommand.CreateFromObservable(
                (AuthenticationModel m) => CheckCode(authenticator, m.ConfirmCode, m.FirstName, m.LastName),
                canCheckCode,
                RxApp.MainThreadScheduler);
            
            model.CheckPasswordCommand = ReactiveCommand.CreateFromObservable(
                (AuthenticationModel m) => CheckPassword(authenticator, m.Password),
                canCheckPassword,
                RxApp.MainThreadScheduler);

            return authenticator.ObserveState()
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(state => HandleState(model, state));
        }

        private static void HandleState(AuthenticationModel model, TdApi.AuthorizationState state)
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

        private static void OnWaitingPhoneNumber(AuthenticationModel model)
        {
            model.ConfirmIndex = 0;
            model.PasswordIndex = 0;
        }

        private static void OnWaitingConfirmCode(AuthenticationModel model, bool isRegistration)
        {
            model.IsRegistration = isRegistration;
            
            model.ConfirmIndex = 1;
            model.PasswordIndex = 0;
        }

        private static void OnWaitingPassword(AuthenticationModel model)
        {
            model.ConfirmIndex = 1;
            model.PasswordIndex = 1;
        }

        private static IObservable<SendCodeResult> SendCode(
            IAuthenticator authenticator,
            string phoneNumber)
        {
            return authenticator
                .SetPhoneNumber(phoneNumber)
                .Select(_ => new SendCodeResult());
        }

        private static IObservable<CheckCodeResult> CheckCode(
            IAuthenticator authenticator,
            string code,
            string firstName,
            string lastName)
        {
            return authenticator
                .CheckCode(code, firstName, lastName)
                .Select(_ => new CheckCodeResult());
        }

        private static IObservable<CheckPasswordResult> CheckPassword(
            IAuthenticator authenticator,
            string password)
        {
            return authenticator
                .CheckPassword(password)
                .Select(_ => new CheckPasswordResult());
        }
    }
}