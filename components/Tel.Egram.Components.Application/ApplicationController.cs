using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using TdLib;
using Tel.Egram.Authentication;
using Tel.Egram.Components.Authentication;
using Tel.Egram.Components.Workspace;
using Tel.Egram.Gui.Views.Application;
using Tel.Egram.Gui.Views.Application.Popup;
using Tel.Egram.Gui.Views.Application.Startup;
using Tel.Egram.Gui.Views.Authentication;
using Tel.Egram.Gui.Views.Workspace;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Application
{
    public class ApplicationController : BaseController<MainWindowModel>
    {
        private readonly IActivator<AuthenticationControlModel> _authenticationActivator;
        private IController<AuthenticationControlModel> _authenticationController;
        
        private readonly IActivator<WorkspaceControlModel> _workspaceActivator;
        private IController<WorkspaceControlModel> _workspaceController;

        public ApplicationController(
            IAuthenticator authenticator,
            IApplicationPopupController applicationPopupController,
            IActivator<AuthenticationControlModel> authenticationActivator,
            IActivator<WorkspaceControlModel> workspaceActivator)
        {
            _authenticationActivator = authenticationActivator;
            _workspaceActivator = workspaceActivator;
            
            BindAuthenticator(authenticator)
                .DisposeWith(this);

            BindPopup(applicationPopupController)
                .DisposeWith(this);
        }

        private IDisposable BindAuthenticator(IAuthenticator authenticator)
        {
            return authenticator
                .ObserveState()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(state =>
                {   
                    switch (state)
                    {
                        case TdApi.AuthorizationState.AuthorizationStateWaitTdlibParameters _:
                            GoToStartupPage();
                            authenticator.SetupParameters()
                                .Subscribe()
                                .DisposeWith(this);
                            break;
                    
                        case TdApi.AuthorizationState.AuthorizationStateWaitEncryptionKey _:
                            GoToStartupPage();
                            authenticator.CheckEncryptionKey()
                                .Subscribe()
                                .DisposeWith(this);
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
                });
        }

        private IDisposable BindPopup(IApplicationPopupController controller)
        {
            return Observable.FromEventPattern<PopupControlModel>(
                    h => controller.ContextChanged += h,
                    h => controller.ContextChanged -= h)
                .Select(e => e.EventArgs)
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(popupModel =>
                {
                    Model.PopupModel = popupModel ?? new PopupControlModel
                    {
                        IsPopupVisible = false
                    };
                });
        }

        private void GoToStartupPage()
        {
            if (Model.StartupModel == null)
            {
                var startupPageModel = new StartupControlModel();
                Model.StartupModel = startupPageModel;
            }
            
            Model.PageIndex = (int) Page.Initial;

            _authenticationController?.Dispose();
            _workspaceController?.Dispose();
            
            Model.WorkspaceModel = null;
            Model.AuthenticationModel = null;
        }

        private void GoToAuthenticationPage()
        {
            if (Model.AuthenticationModel == null)
            {
                var model = _authenticationActivator.Activate(ref _authenticationController);
                Model.AuthenticationModel = model;
            }
            
            Model.PageIndex = (int) Page.Authentication;

            _workspaceController?.Dispose();

            Model.StartupModel = null;
            Model.WorkspaceModel = null;
        }

        private void GoToWorkspacePage()
        {
            if (Model.WorkspaceModel == null)
            {
                var model = _workspaceActivator.Activate(ref _workspaceController);
                Model.WorkspaceModel = model;
            }
            
            Model.PageIndex = (int) Page.Workspace;
            
            _authenticationController?.Dispose();
            
            Model.StartupModel = null;
            Model.AuthenticationModel = null;
        }

        public override void Dispose()
        {
            _authenticationController?.Dispose();
            _workspaceController?.Dispose();
            
            base.Dispose();
        }
    }
}