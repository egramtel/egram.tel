using System;
using ReactiveUI;
using Tel.Egram.Components.Authentication;
using Tel.Egram.Components.Workspace;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Application
{
    public class ApplicationContext : ReactiveObject, IDisposable
    {
        private const int PageLoading = 0;
        private const int PageAuthentication = 1;
        private const int PageWorkspace = 2;
        
        private readonly IFactory<WorkspaceContext> _workspaceContextFactory;
        private readonly IFactory<AuthenticationContext> _authenticationContextFactory;
        private readonly AuthenticationInteractor _authenticationInteractor;

        private readonly IDisposable _authenticationSubscription;

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
            IFactory<WorkspaceContext> workspaceContextFactory,
            IFactory<AuthenticationContext> authenticationContextFactory,
            IFactory<ApplicationContext, AuthenticationInteractor> authenticationInteractorFactory
            )
        {
            _workspaceContextFactory = workspaceContextFactory;
            _authenticationContextFactory = authenticationContextFactory;

            _authenticationInteractor = authenticationInteractorFactory.Create(this);
            _authenticationSubscription = _authenticationInteractor.Bind(this);
        }

        public void InitLoading()
        {
            PageIndex = PageLoading;
            
            AuthenticationContext?.Dispose();
            AuthenticationContext = null;
            
            WorkspaceContext?.Dispose();
            WorkspaceContext = null;
        }

        public void InitAuthentication()
        {
            PageIndex = PageAuthentication;

            if (AuthenticationContext == null)
            {
                WorkspaceContext?.Dispose();
                WorkspaceContext = null;

                AuthenticationContext = _authenticationContextFactory.Create();
            }
        }

        public void InitWorkspace()
        {
            PageIndex = PageWorkspace;

            if (WorkspaceContext == null)
            {
                AuthenticationContext?.Dispose();
                AuthenticationContext = null;

                WorkspaceContext = _workspaceContextFactory.Create();
            }
        }

        public void Dispose()
        {
            _authenticationSubscription.Dispose();
            _authenticationInteractor.Dispose();
            
            WorkspaceContext?.Dispose();
            AuthenticationContext?.Dispose();
        }
    }
}