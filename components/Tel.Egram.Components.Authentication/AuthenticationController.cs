using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using ReactiveUI;
using TdLib;
using Tel.Egram.Authentication;
using Tel.Egram.Gui.Views.Authentication;

namespace Tel.Egram.Components.Authentication
{
    public class AuthenticationController
        : BaseController, IAuthenticationController
    {
        public AuthenticationController(
            AuthenticationPageModel authenticationPageModel,
            IAuthenticator authenticator)
        {
            BindAuthenticator(authenticationPageModel, authenticator)
                .DisposeWith(this);
        }

        private IDisposable BindAuthenticator(
            AuthenticationPageModel model,
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
            
            var stateObservable = authenticator
                .ObserveState()
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler);
            
            return stateObservable
                .Subscribe(state =>
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
                });
        }
        
        private void OnWaitingPhoneNumber(AuthenticationPageModel model)
        {
            model.ConfirmIndex = 0;
            model.PasswordIndex = 0;
        }

        private void OnWaitingConfirmCode(AuthenticationPageModel model, bool registration)
        {
            model.ConfirmIndex = 1;
            model.PasswordIndex = 0;
        }

        private void OnWaitingPassword(AuthenticationPageModel model)
        {
            model.ConfirmIndex = 1;
            model.PasswordIndex = 1;
        }
    }
}