using System;
using System.Reactive;
using Avalonia.Threading;
using Egram.Components.Authorization;
using Egram.Components.Navigation;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace Egram.Components.Authorization
{
    public class AuthorizationContext : ReactiveObject, IDisposable
    {
        private readonly Authorizer _authorizer;
        private readonly Navigator _navigator;

        private readonly IDisposable _authorizationStateSubscription;
        
        public AuthorizationContext(
            Authorizer authorizer,
            Navigator navigator
            )
        {
            _authorizer = authorizer;
            _navigator = navigator;

            InitCommands();
            
            _authorizationStateSubscription = _authorizer
                .States()
                .Subscribe(ObserveAuthorizationState);
        }
        
        private int _activeFormIndex;
        public int ActiveFormIndex
        {
            get => _activeFormIndex;
            set => this.RaiseAndSetIfChanged(ref _activeFormIndex, value);
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set => this.RaiseAndSetIfChanged(ref _phoneNumber, value);
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

        private string _verificationCode;
        public string VerificationCode
        {
            get => _verificationCode;
            set => this.RaiseAndSetIfChanged(ref _verificationCode, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }
        
        public ReactiveCommand<Unit, Unit> RequestCodeCommand { get; private set; }
            
        public ReactiveCommand<Unit, Unit> SignupCommand { get; private set; }
            
        public ReactiveCommand<Unit, Unit> VerifyCodeCommand { get; private set; }

        public ReactiveCommand<Unit, Unit> CheckPasswordCommand { get; private set; }

        private void InitCommands()
        {
            // Phone number form
            RequestCodeCommand = ReactiveCommand.CreateFromTask(async () =>
                {
                    await _authorizer.SetPhoneNumber(_phoneNumber);
                },
                null,
                AvaloniaScheduler.Instance);
            
            // Signup form
            SignupCommand = ReactiveCommand.Create(() => {},
                null,
                AvaloniaScheduler.Instance);
            SignupCommand.Subscribe(_ =>
            {
                ActiveFormIndex = 2;
            });
            
            // Verify code form
            VerifyCodeCommand = ReactiveCommand.CreateFromTask(async () =>
                {
                    if (string.IsNullOrEmpty(_firstName))
                    {
                        await _authorizer.CheckCode(_verificationCode);
                    }
                    else
                    {
                        await _authorizer.CheckCode(_verificationCode, _firstName, _lastName);
                    }
                },
                null,
                AvaloniaScheduler.Instance);
            
            // Password form
            CheckPasswordCommand = ReactiveCommand.CreateFromTask(async () =>
                {
                    await _authorizer.CheckPassword(_password);
                },
                null,
                AvaloniaScheduler.Instance);
        }

        private void ObserveAuthorizationState(TD.AuthorizationState state)
        {
            switch (state)
            {       
                case TD.AuthorizationState.AuthorizationStateWaitPhoneNumber _:
                    ActiveFormIndex = 0;
                    break;
                        
                case TD.AuthorizationState.AuthorizationStateWaitCode waitCode:
                    ActiveFormIndex = waitCode.IsRegistered ? 2 : 1;
                    break;
                        
                case TD.AuthorizationState.AuthorizationStateWaitPassword _:
                    ActiveFormIndex = 3;
                    break;
            }
        }

        public void Dispose()
        {
            _authorizationStateSubscription.Dispose();
        }
    }

    public class AuthorizationContextFactory
    {
        private readonly IServiceProvider _provider;

        public AuthorizationContextFactory(IServiceProvider provider)
        {
            _provider = provider;
        }
        
        public AuthorizationContext Create()
        {
            return _provider.GetService<AuthorizationContext>();
        }
    }
}