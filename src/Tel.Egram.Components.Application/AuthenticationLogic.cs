using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using TdLib;
using Tel.Egram.Authentication;
using Tel.Egram.Components.Application.Startup;
using Tel.Egram.Components.Authentication;
using Tel.Egram.Components.Workspace;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Application
{
    public static class AuthenticationLogic
    {
        public static IDisposable BindAuthentication(
            this MainWindowModel model,
            IAuthenticator authenticator = null)
        {
            var disposable = new CompositeDisposable();
            
            var stateUpdates = authenticator
                .ObserveState()
                .ObserveOn(RxApp.MainThreadScheduler);
            
            stateUpdates.OfType<TdApi.AuthorizationState.AuthorizationStateWaitTdlibParameters>()
                .SelectMany(_ => authenticator.SetupParameters())
                .Subscribe(state => HandleState(model, state))
                .DisposeWith(disposable);

            stateUpdates.OfType<TdApi.AuthorizationState.AuthorizationStateWaitEncryptionKey>()
                .SelectMany(_ => authenticator.CheckEncryptionKey())
                .Subscribe(state => HandleState(model, state))
                .DisposeWith(disposable);

            stateUpdates.OfType<TdApi.AuthorizationState.AuthorizationStateWaitPhoneNumber>()
                .Subscribe(state => HandleState(model, state))
                .DisposeWith(disposable);
            
            stateUpdates.OfType<TdApi.AuthorizationState.AuthorizationStateWaitCode>()
                .Subscribe(state => HandleState(model, state))
                .DisposeWith(disposable);
            
            stateUpdates.OfType<TdApi.AuthorizationState.AuthorizationStateWaitPassword>()
                .Subscribe(state => HandleState(model, state))
                .DisposeWith(disposable);
            
            stateUpdates.OfType<TdApi.AuthorizationState.AuthorizationStateReady>()
                .Subscribe(state => HandleState(model, state))
                .DisposeWith(disposable);

            return disposable;
        }

        private static void HandleState(MainWindowModel model, TdApi.AuthorizationState state)
        {
            switch (state)
            {
                case TdApi.AuthorizationState.AuthorizationStateWaitTdlibParameters _:
                case TdApi.AuthorizationState.AuthorizationStateWaitEncryptionKey _:
                    GoToStartupPage(model);
                    break;
                
                case TdApi.AuthorizationState.AuthorizationStateWaitPhoneNumber _:
                case TdApi.AuthorizationState.AuthorizationStateWaitCode _:
                case TdApi.AuthorizationState.AuthorizationStateWaitPassword _:
                    GoToAuthenticationPage(model);
                    break;
            
                case TdApi.AuthorizationState.AuthorizationStateReady _:
                    GoToWorkspacePage(model);
                    break;
            }
        }
        
        private static void GoToStartupPage(MainWindowModel model)
        {
            if (model.StartupModel == null)
            {
                model.StartupModel = new StartupModel();
            }

            model.WorkspaceModel = null;
            model.AuthenticationModel = null;
            
            model.PageIndex = (int) Page.Initial;
        }

        private static void GoToAuthenticationPage(MainWindowModel model)
        {
            if (model.AuthenticationModel == null)
            {
                model.AuthenticationModel = new AuthenticationModel();
            }
            
            model.PageIndex = (int) Page.Authentication;

            model.StartupModel = null;
            model.WorkspaceModel = null;
        }

        private static void GoToWorkspacePage(MainWindowModel model)
        {
            if (model.WorkspaceModel == null)
            {
                model.WorkspaceModel = new WorkspaceModel();
            }
            
            model.PageIndex = (int) Page.Workspace;
            
            model.StartupModel = null;
            model.AuthenticationModel = null;
        }
    }
}