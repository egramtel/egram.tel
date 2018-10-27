using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using ReactiveUI;
using TdLib;
using Tel.Egram.Authentication;
using Tel.Egram.Models.Authentication;

namespace Tel.Egram.Components.Authentication
{
    public class AuthenticationController : BaseController<AuthenticationModel>
    {
        public AuthenticationController(IAuthenticator authenticator)
        {
            BindAuthenticator(authenticator)
                .DisposeWith(this);
        }

        private IDisposable BindAuthenticator(IAuthenticator authenticator)
        {
            var canSendCode = Model
                .WhenAnyValue(x => x.PhoneNumber)
                .Select(phone => !string.IsNullOrWhiteSpace(phone));
            
            var canCheckCode = Model
                .WhenAnyValue(x => x.ConfirmCode)
                .Select(code => !string.IsNullOrWhiteSpace(code));
            
            var canCheckPassword = Model
                .WhenAnyValue(x => x.Password)
                .Select(password => !string.IsNullOrWhiteSpace(password));
            
            Model.SendCodeCommand = ReactiveCommand.CreateFromObservable(
                () => authenticator.SetPhoneNumber(Model.PhoneNumber),
                canSendCode, RxApp.MainThreadScheduler);

            Model.CheckCodeCommand = ReactiveCommand.CreateFromObservable(
                () => authenticator.CheckCode(Model.ConfirmCode, Model.FirstName, Model.LastName),
                canCheckCode, RxApp.MainThreadScheduler);
            
            Model.CheckPasswordCommand = ReactiveCommand.CreateFromObservable(
                () => authenticator.CheckPassword(Model.Password),
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
                            OnWaitingPhoneNumber();
                            break;
                        
                        case TdApi.AuthorizationState.AuthorizationStateWaitCode wait:
                            OnWaitingConfirmCode(!wait.IsRegistered);
                            break;
                        
                        case TdApi.AuthorizationState.AuthorizationStateWaitPassword _:
                            OnWaitingPassword();
                            break;
                    }
                });
        }
        
        private void OnWaitingPhoneNumber()
        {
            Model.ConfirmIndex = 0;
            Model.PasswordIndex = 0;
        }

        private void OnWaitingConfirmCode(bool registration)
        {
            Model.ConfirmIndex = 1;
            Model.PasswordIndex = 0;
        }

        private void OnWaitingPassword()
        {
            Model.ConfirmIndex = 1;
            Model.PasswordIndex = 1;
        }
    }
}