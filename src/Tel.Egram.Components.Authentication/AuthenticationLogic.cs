using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using Splat;
using TdLib;
using Tel.Egram.Authentication;

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
                () => authenticator.SetPhoneNumber(model.PhoneNumber),
                canSendCode, RxApp.MainThreadScheduler);

            model.CheckCodeCommand = ReactiveCommand.CreateFromObservable(
                () => authenticator.CheckCode(model.ConfirmCode, model.FirstName, model.LastName),
                canCheckCode, RxApp.MainThreadScheduler);
            
            model.CheckPasswordCommand = ReactiveCommand.CreateFromObservable(
                () => authenticator.CheckPassword(model.Password),
                canCheckPassword, RxApp.MainThreadScheduler);

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

        private static void OnWaitingConfirmCode(AuthenticationModel model, bool registration)
        {
            model.ConfirmIndex = 1;
            model.PasswordIndex = 0;
        }

        private static void OnWaitingPassword(AuthenticationModel model)
        {
            model.ConfirmIndex = 1;
            model.PasswordIndex = 1;
        }
    }
}