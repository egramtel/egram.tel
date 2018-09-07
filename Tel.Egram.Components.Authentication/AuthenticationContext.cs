using System;
using System.Reactive;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Authentication
{
    public class AuthenticationContext : ReactiveObject, IDisposable
    {
        private readonly AuthenticationInteractor _authenticationInteractor;
        private readonly IDisposable _authenticationInteractorSubscription;
        
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
            IFactory<AuthenticationContext, AuthenticationInteractor> authenticationInteractorFactory)
        {
            _authenticationInteractor = authenticationInteractorFactory.Create(this);
            
            SendCodeCommand = ReactiveCommand.Create<Unit, Unit>(_ =>  Unit.Default,
                null,
                AvaloniaScheduler.Instance);
            
            CheckCodeCommand = ReactiveCommand.Create<Unit, Unit>(_ => Unit.Default,
                null,
                AvaloniaScheduler.Instance);
            
            CheckPasswordCommand = ReactiveCommand.Create<Unit, Unit>(_ => Unit.Default,
                null,
                AvaloniaScheduler.Instance);

            _authenticationInteractorSubscription = _authenticationInteractor.Bind(this);
        }

        public void OnWaitingPhoneNumber()
        {
            ConfirmIndex = 0;
            PasswordIndex = 0;
        }

        public void OnWaitingConfirmCode(bool registration)
        {
            ConfirmIndex = 1;
            PasswordIndex = 0;
        }

        public void OnWaitingPassword()
        {
            ConfirmIndex = 1;
            PasswordIndex = 1;
        }

        public void Dispose()
        {
            _authenticationInteractor.Dispose();
            _authenticationInteractorSubscription.Dispose();
        }
    }
}