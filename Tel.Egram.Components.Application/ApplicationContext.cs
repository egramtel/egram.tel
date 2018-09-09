using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Threading;
using ReactiveUI;
using TdLib;
using Tel.Egram.Authentication;
using Tel.Egram.Components.Authentication;
using Tel.Egram.Components.Workspace;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Application
{
    public class ApplicationContext : ReactiveObject, IDisposable
    {
        private readonly CompositeDisposable _contextDisposable = new CompositeDisposable();

        private readonly IAuthenticator _authenticator;
        private readonly IFactory<WorkspaceContext> _workspaceContextFactory;
        private readonly IFactory<AuthenticationContext> _authenticationContextFactory;

        private WorkspaceContext _workspaceContext;
        public WorkspaceContext WorkspaceContext
        {
            get => _workspaceContext;
            set => this.RaiseAndSetIfChanged(ref _workspaceContext, value);
        }

        private AuthenticationContext _authenticationContext;
        public AuthenticationContext AuthenticationContext
        {
            get => _authenticationContext;
            set => this.RaiseAndSetIfChanged(ref _authenticationContext, value);
        }

        private int _pageIndex;
        public int PageIndex
        {
            get => _pageIndex;
            set => this.RaiseAndSetIfChanged(ref _pageIndex, value);
        }

        private string _windowTitle = "Egram";
        public string WindowTitle
        {
            get => _windowTitle;
            set => this.RaiseAndSetIfChanged(ref _windowTitle, value);
        }
        
        public ApplicationContext(
            IAuthenticator authenticator,
            IFactory<WorkspaceContext> workspaceContextFactory,
            IFactory<AuthenticationContext> authenticationContextFactory
            )
        {
            _authenticator = authenticator;
            _workspaceContextFactory = workspaceContextFactory;
            _authenticationContextFactory = authenticationContextFactory;
            
            _authenticator.ObserveState()
                .ObserveOn(AvaloniaScheduler.Instance)
                .Subscribe(state =>
                {
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