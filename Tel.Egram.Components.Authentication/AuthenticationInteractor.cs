using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using TdLib;
using Tel.Egram.TdLib;

namespace Tel.Egram.Components.Authentication
{
    public class AuthenticationInteractor : IDisposable
    {
        private readonly TdAgent _agent;

        public AuthenticationInteractor(
            TdAgent agent
            )
        {
            _agent = agent;
        }

        public IDisposable Bind(AuthenticationContext context)
        {
            var updatesSubscription = _agent.Updates.OfType<TdApi.Update.UpdateAuthorizationState>()
                .Select(update => update.AuthorizationState)
                .ObserveOn(AvaloniaScheduler.Instance)
                .Subscribe(state =>
                {
                    switch (state)
                    {
                        case TdApi.AuthorizationState.AuthorizationStateWaitPhoneNumber waitPhoneNumber:
                            context.OnWaitingPhoneNumber();
                            break;
                        
                        case TdApi.AuthorizationState.AuthorizationStateWaitCode waitCode:
                            context.OnWaitingConfirmCode(!waitCode.IsRegistered);
                            break;
                        
                        case TdApi.AuthorizationState.AuthorizationStateWaitPassword waitPassword:
                            context.OnWaitingPassword();
                            break;
                    }
                });
            
            var sendCodeSubscription = context.SendCodeCommand
                .ObserveOn(AvaloniaScheduler.Instance)
                    .SelectMany(_ =>
                    _agent.Execute(new TdApi.SetAuthenticationPhoneNumber
                    {
                        PhoneNumber = context.PhoneNumber
                    }))
                .Subscribe();

            var checkCodeSubscription = context.CheckCodeCommand
                .ObserveOn(AvaloniaScheduler.Instance)
                .SelectMany(_ =>
                    _agent.Execute(new TdApi.CheckAuthenticationCode
                    {
                        Code = context.ConfirmCode,
                        FirstName = context.FirstName,
                        LastName = context.LastName
                    }))
                .Subscribe();

            var checkPasswordSubscription = context.CheckPasswordCommand
                .ObserveOn(AvaloniaScheduler.Instance)
                .SelectMany(_ =>
                    _agent.Execute(new TdApi.CheckAuthenticationPassword
                    {
                        Password = context.Password
                    }))
                .Subscribe();
            
            return Disposable.Create(() =>
            {
                updatesSubscription.Dispose();
                sendCodeSubscription.Dispose();
                checkCodeSubscription.Dispose();
                checkPasswordSubscription.Dispose();
            });
        }

        public void Dispose()
        {
            
        }
    }
}