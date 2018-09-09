using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using TdLib;
using Tel.Egram.Authentication;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Authentication
{
    public class AuthenticationContext : ReactiveObject, IDisposable
    {
        private readonly CompositeDisposable _contextDisposable = new CompositeDisposable();
        
        private readonly IAuthenticator _authenticator;
        
        public ReactiveCommand<Unit, Unit> SendCodeCommand { get; }
        
        public ReactiveCommand<Unit, Unit> CheckCodeCommand { get; }
        
        public ReactiveCommand<Unit, Unit> CheckPasswordCommand { get; }
        
        private int _confirmIndex;
        public int ConfirmIndex
        {
            get => _confirmIndex;
            set => this.RaiseAndSetIfChanged(ref _confirmIndex, value);
        }

        private int _passwordIndex;
        public int PasswordIndex
        {
            get => _passwordIndex;
            set => this.RaiseAndSetIfChanged(ref _passwordIndex, value);
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set => this.RaiseAndSetIfChanged(ref _phoneNumber, value);
        }

        private string _confirmCode;
        public string ConfirmCode
        {
            get => _confirmCode;
            set => this.RaiseAndSetIfChanged(ref _confirmCode, value);
        }

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set => this.RaiseAndSetIfChanged(ref _firstName, value);
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set => this.RaiseAndSetIfChanged(ref _lastName, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        public AuthenticationContext(
            IAuthenticator authenticator
            )
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