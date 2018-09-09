using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using PropertyChanged;
using ReactiveUI;
using TdLib;
using Tel.Egram.Authentication;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Authentication
{
    [AddINotifyPropertyChangedInterface]
    public class AuthenticationContext : IDisposable
    {
        private readonly CompositeDisposable _contextDisposable = new CompositeDisposable();
        private readonly IAuthenticator _authenticator;
        
        public ReactiveCommand<Unit, Unit> SendCodeCommand { get; }
        public ReactiveCommand<Unit, Unit> CheckCodeCommand { get; }
        public ReactiveCommand<Unit, Unit> CheckPasswordCommand { get; }
        
        public string PhoneNumber { get; set; }
        public string ConfirmCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        
        public int ConfirmIndex { get; set; }
        public int PasswordIndex { get; set; }

        public AuthenticationContext(IAuthenticator authenticator)
        {
            _authenticator = authenticator;
            
            SendCodeCommand = ReactiveCommand.Create<Unit, Unit>(_ =>  Unit.Default,
                null,
                AvaloniaScheduler.Instance);
            
            CheckCodeCommand = ReactiveCommand.Create<Unit, Unit>(_ => Unit.Default,
                null,
                AvaloniaScheduler.Instance);
            
            CheckPasswordCommand = ReactiveCommand.Create<Unit, Unit>(_ => Unit.Default,
                null,
                AvaloniaScheduler.Instance);

            SendCodeCommand
                .ObserveOn(AvaloniaScheduler.Instance)
                .SelectMany(_ => _authenticator.SetPhoneNumber(PhoneNumber) )
                .Subscribe()
                .DisposeWith(_contextDisposable);

            CheckCodeCommand
                .ObserveOn(AvaloniaScheduler.Instance)
                .SelectMany(_ => _authenticator.CheckCode(ConfirmCode, FirstName, LastName))
                .Subscribe()
                .DisposeWith(_contextDisposable);

            CheckPasswordCommand
                .ObserveOn(AvaloniaScheduler.Instance)
                .SelectMany(_ => _authenticator.CheckPassword(Password))
                .Subscribe()
                .DisposeWith(_contextDisposable);
            
            _authenticator.ObserveState()
                .ObserveOn(AvaloniaScheduler.Instance)
                .Subscribe(state =>
                {
                    switch (state)
                    {
                        case TdApi.AuthorizationState.AuthorizationStateWaitPhoneNumber waitPhoneNumber:
                            OnWaitingPhoneNumber();
                            break;
                        
                        case TdApi.AuthorizationState.AuthorizationStateWaitCode waitCode:
                            OnWaitingConfirmCode(!waitCode.IsRegistered);
                            break;
                        
                        case TdApi.AuthorizationState.AuthorizationStateWaitPassword waitPassword:
                            OnWaitingPassword();
                            break;
                    }
                })
                .DisposeWith(_contextDisposable);
        }

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

        public void Dispose()
        {
            _contextDisposable.Dispose();
        }
    }
}