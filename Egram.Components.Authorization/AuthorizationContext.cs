using System;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Threading;
using Egram.Components.Navigation;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI.Fody.Helpers;
using ReactiveUI;
using System.Threading.Tasks;

namespace Egram.Components.Authorization
{
    public class AuthorizationContext : ReactiveObject, IDisposable
    {
        private readonly IDisposable _authorizationStateSubscription;
        private readonly Authorizer _authorizer;
        private readonly Navigator _navigator;

        public ReactiveCommand<Unit, Unit> RequestCodeCommand { get; }
        public ReactiveCommand<Unit, Unit> SignupCommand { get; }
        public ReactiveCommand<Unit, Unit> VerifyCodeCommand { get; }
        public ReactiveCommand<Unit, Unit> CheckPasswordCommand { get; }

        [Reactive] public string VerificationCode { get; set; }
        [Reactive] public int ActiveFormIndex { get; set; }
        [Reactive] public string PhoneNumber { get; set; }
        [Reactive] public string FirstName { get; set; }
        [Reactive] public string LastName { get; set; }
        [Reactive] public string Password { get; set; }

        public AuthorizationContext(Authorizer authorizer, Navigator navigator)
        {
            _authorizer = authorizer ?? throw new ArgumentNullException(nameof(authorizer));
            _navigator = navigator ?? throw new ArgumentNullException(nameof(navigator));

            var canRequestCode = this
                .WhenAnyValue(x => x.PhoneNumber)
                .Select(x => !string.IsNullOrWhiteSpace(x));

            RequestCodeCommand = ReactiveCommand.CreateFromTask(
                () => _authorizer.SetPhoneNumber(PhoneNumber), 
                canRequestCode, AvaloniaScheduler.Instance);

            SignupCommand = ReactiveCommand.Create(
                () => { ActiveFormIndex = 2; },
                null, AvaloniaScheduler.Instance);

            var canVerifyCode = this
                .WhenAnyValue(x => x.VerificationCode)
                .Select(x => !string.IsNullOrWhiteSpace(x));

            VerifyCodeCommand = ReactiveCommand.CreateFromTask(
                VerifyCode, canVerifyCode, AvaloniaScheduler.Instance);

            var canCheckPassword = this
                .WhenAnyValue(x => x.Password)
                .Select(x => !string.IsNullOrWhiteSpace(x));

            CheckPasswordCommand = ReactiveCommand.CreateFromTask(
                () => authorizer.CheckPassword(Password),
                canCheckPassword, AvaloniaScheduler.Instance);

            _authorizationStateSubscription = _authorizer
                .States()
                .Subscribe(ObserveAuthorizationState);
        }

        private Task VerifyCode()
        {
            if (string.IsNullOrEmpty(FirstName))
            {
                return _authorizer.CheckCode(VerificationCode);
            }
            else
            {
                return _authorizer.CheckCode(VerificationCode, FirstName, LastName);
            }
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