using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using PropertyChanged;
using ReactiveUI;
using TdLib;
using Tel.Egram.Authentication;
using Tel.Egram.Components.Authentication;
using Tel.Egram.Components.Popup;
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

        public AuthenticationContext AuthenticationContext { get; set; }
        
        public WorkspaceContext WorkspaceContext { get; set; }
        
        public PopupContext PopupContext { get; set; }
        
        public string WindowTitle { get; set; } = "Egram";
        
        public int PageIndex { get; set; }
        
        public ApplicationContext(
            IFactory<AuthenticationContext> authenticationContextFactory,
            IFactory<WorkspaceContext> workspaceContextFactory,
            IApplicationPopupController popupController,
            IAuthenticator authenticator)
        {
            _authenticationContextFactory = authenticationContextFactory;
            _workspaceContextFactory = workspaceContextFactory;
            
            SubscribeToState(authenticator);
            SubscribeToPopup(popupController);
        }

        private void SubscribeToPopup(IApplicationPopupController controller)
        {
            PopupContext = new HiddenPopupContext(controller);
            
            Observable.FromEventPattern<PopupContext>(
                h => controller.ContextChanged += h,
                h => controller.ContextChanged -= h)
                .Select(e => e.EventArgs)
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(popupContext =>
                {
                    PopupContext.Dispose();
                    PopupContext = popupContext ?? new HiddenPopupContext(controller);
                })
                .DisposeWith(_contextDisposable);
        }

        private void SubscribeToState(IAuthenticator authenticator)
        {
            authenticator
                .ObserveState()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(state =>
                {   
                    switch (state)
                    {
                        case TdApi.AuthorizationState.AuthorizationStateWaitTdlibParameters _:
                            GoToInitialPage();
                            authenticator.SetupParameters()
                                .Subscribe()
                                .DisposeWith(_contextDisposable);
                            break;
                    
                        case TdApi.AuthorizationState.AuthorizationStateWaitEncryptionKey _:
                            GoToInitialPage();
                            authenticator.CheckEncryptionKey()
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