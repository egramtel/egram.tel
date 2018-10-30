using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using TdLib;
using Tel.Egram.Authentication;
using Tel.Egram.Models.Authentication;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Authentication
{
    public class AuthenticationController : Controller<AuthenticationModel>, ISupportsActivation
    {
        public ViewModelActivator Activator { get; }
        
        public AuthenticationController(
            ISchedulers schedulers,
            IAuthenticator authenticator)
        {
            Activator = new ViewModelActivator();
            this.WhenActivated(disposables =>
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
                    canSendCode, schedulers.Main);
    
                Model.CheckCodeCommand = ReactiveCommand.CreateFromObservable(
                    () => authenticator.CheckCode(Model.ConfirmCode, Model.FirstName, Model.LastName),
                    canCheckCode, schedulers.Main);
                
                Model.CheckPasswordCommand = ReactiveCommand.CreateFromObservable(
                    () => authenticator.CheckPassword(Model.Password),
                    canCheckPassword, schedulers.Main);
                
                var stateObservable = authenticator
                    .ObserveState()
                    .SubscribeOn(schedulers.Pool)
                    .ObserveOn(schedulers.Main);
                
                stateObservable
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
                    })
                    .DisposeWith(disposables);
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