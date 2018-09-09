using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using PropertyChanged;
using ReactiveUI;
using TdLib;
using Tel.Egram.Authentication;
using Tel.Egram.Components.Authentication;
using Tel.Egram.Components.Workspace;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Application
{
    [AddINotifyPropertyChangedInterface]
    public class ApplicationContext : IDisposable
    {
        private readonly CompositeDisposable _contextDisposable = new CompositeDisposable();
        private readonly IFactory<AuthenticationContext> _authenticationContextFactory;
        private readonly IFactory<WorkspaceContext> _workspaceContextFactory;
        private readonly IAuthenticator _authenticator;

        public AuthenticationContext AuthenticationContext { get; set; }
        public WorkspaceContext WorkspaceContext { get; set; }
        public string WindowTitle { get; set; } = "Egram";
        public int PageIndex { get; set; }
        
        public ApplicationContext(
            IFactory<AuthenticationContext> authenticationContextFactory,
            IFactory<WorkspaceContext> workspaceContextFactory,
            IAuthenticator authenticator)
        {
            _authenticationContextFactory = authenticationContextFactory;
            _workspaceContextFactory = workspaceContextFactory;
            _authenticator = authenticator;
            
            _authenticator
                .ObserveState()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(state =>
                {   
                    Console.WriteLine("State update received.");
                    Console.WriteLine(state.DataType);
                    switch (state)
                    {
                        case TdApi.AuthorizationState.AuthorizationStateWaitTdlibParameters _:
                            GoToInitialPage();
                            _authenticator.SetupParameters()
                                .Subscribe()
                                .DisposeWith(_contextDisposable);
                            break;
                    
                        case TdApi.AuthorizationState.AuthorizationStateWaitEncryptionKey _:
                            GoToInitialPage();
                            _authenticator.CheckEncryptionKey()
                                .Subscribe()
                                .DisposeWith(_contextDisposable);
                            break;
                    
                        case TdApi.AuthorizationState.AuthorizationStateWaitPhoneNumber _:
                        case TdApi.AuthorizationState.AuthorizationStateWaitCode _:
                        case TdApi.AuthorizationState.AuthorizationStateWaitPassword _:
                            GoToAuthenticationPage();
                            break;
                
                        case TdApi.AuthorizationState.AuthorizationStateReady _:
                            GoToWorkspacePage();
                            break;
                    }
                })
                .DisposeWith(_contextDisposable);
        }

        private void GoToInitialPage()
        {
            PageIndex = (int) Page.Initial;
            
            AuthenticationContext?.Dispose();
            AuthenticationContext = null;
            
            WorkspaceContext?.Dispose();
            WorkspaceContext = null;
        }

        private void GoToAuthenticationPage()
        {
            PageIndex = (int) Page.Authentication;

            if (AuthenticationContext == null)
            {
                WorkspaceContext?.Dispose();
                WorkspaceContext = null;

                AuthenticationContext = _authenticationContextFactory.Create();
            }
        }

        private void GoToWorkspacePage()
        {
            PageIndex = (int) Page.Workspace;

            if (WorkspaceContext == null)
            {
                AuthenticationContext?.Dispose();
                AuthenticationContext = null;

                WorkspaceContext = _workspaceContextFactory.Create();
            }
        }

        public void Dispose()
        {   
            WorkspaceContext?.Dispose();
            AuthenticationContext?.Dispose();
            
            _contextDisposable.Dispose();
        }
    }
}