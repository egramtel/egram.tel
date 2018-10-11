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
    public class ApplicationModel : IDisposable
    {
        private readonly CompositeDisposable _modelDisposable = new CompositeDisposable();
        private readonly IFactory<AuthenticationModel> _authenticationModelFactory;
        private readonly IFactory<WorkspaceModel> _workspaceModelFactory;

        public AuthenticationModel AuthenticationModel { get; set; }
        
        public WorkspaceModel WorkspaceModel { get; set; }
        
        public PopupModel PopupModel { get; set; }
        
        public string WindowTitle { get; set; } = "Egram";
        
        public int PageIndex { get; set; }
        
        public ApplicationModel(
            IFactory<AuthenticationModel> authenticationModelFactory,
            IFactory<WorkspaceModel> workspaceModelFactory,
            IApplicationPopupController popupController,
            IAuthenticator authenticator)
        {
            _authenticationModelFactory = authenticationModelFactory;
            _workspaceModelFactory = workspaceModelFactory;
            
            SubscribeToState(authenticator);
            SubscribeToPopup(popupController);
        }

        private void SubscribeToPopup(IApplicationPopupController controller)
        {
            PopupModel = new HiddenPopupModel(controller);
            
            Observable.FromEventPattern<PopupModel>(
                h => controller.ContextChanged += h,
                h => controller.ContextChanged -= h)
                .Select(e => e.EventArgs)
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(popupModel =>
                {
                    PopupModel.Dispose();
                    PopupModel = popupModel ?? new HiddenPopupModel(controller);
                })
                .DisposeWith(_modelDisposable);
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
                                .DisposeWith(_modelDisposable);
                            break;
                    
                        case TdApi.AuthorizationState.AuthorizationStateWaitEncryptionKey _:
                            GoToInitialPage();
                            authenticator.CheckEncryptionKey()
                                .Subscribe()
                                .DisposeWith(_modelDisposable);
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
                .DisposeWith(_modelDisposable);
        }

        private void GoToInitialPage()
        {
            PageIndex = (int) Page.Initial;
            
            AuthenticationModel?.Dispose();
            AuthenticationModel = null;
            
            WorkspaceModel?.Dispose();
            WorkspaceModel = null;
        }

        private void GoToAuthenticationPage()
        {
            PageIndex = (int) Page.Authentication;

            if (AuthenticationModel == null)
            {
                WorkspaceModel?.Dispose();
                WorkspaceModel = null;

                AuthenticationModel = _authenticationModelFactory.Create();
            }
        }

        private void GoToWorkspacePage()
        {
            PageIndex = (int) Page.Workspace;

            if (WorkspaceModel == null)
            {
                AuthenticationModel?.Dispose();
                AuthenticationModel = null;

                WorkspaceModel = _workspaceModelFactory.Create();
            }
        }

        public void Dispose()
        {   
            PopupModel?.Dispose();
            WorkspaceModel?.Dispose();
            AuthenticationModel?.Dispose();
            
            _modelDisposable.Dispose();
        }
    }
}