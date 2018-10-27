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
        private readonly IActivator<AuthenticationPageModel> _authenticationActivator;
        private IController<AuthenticationPageModel> _authenticationController;
        
        private readonly IActivator<WorkspacePageModel> _workspaceActivator;
        private IController<WorkspacePageModel> _workspaceController;

        public ApplicationController(
            IAuthenticator authenticator,
            IApplicationPopupController applicationPopupController,
            IActivator<AuthenticationPageModel> authenticationActivator,
            IActivator<WorkspacePageModel> workspaceActivator)
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
                    Model.PopupControlModel = popupModel ?? new PopupControlModel
                    {
                        IsPopupVisible = false
                    };
                });
        }

        private void GoToStartupPage()
        {
            if (Model.StartupPageModel == null)
            {
                var startupPageModel = new StartupPageModel();
                Model.StartupPageModel = startupPageModel;
            }
            
            Model.PageIndex = (int) Page.Initial;

            _authenticationController?.Dispose();
            _workspaceController?.Dispose();
            
            Model.WorkspacePageModel = null;
            Model.AuthenticationPageModel = null;
        }

        private void GoToAuthenticationPage()
        {
            if (Model.AuthenticationPageModel == null)
            {
                var model = _authenticationActivator.Activate(ref _authenticationController);
                Model.AuthenticationPageModel = model;
            }
            
            Model.PageIndex = (int) Page.Authentication;

            _workspaceController?.Dispose();

            Model.StartupPageModel = null;
            Model.WorkspacePageModel = null;
        }

        private void GoToWorkspacePage()
        {
            if (Model.WorkspacePageModel == null)
            {
                var model = _workspaceActivator.Activate(ref _workspaceController);
                Model.WorkspacePageModel = model;
            }
            
            Model.PageIndex = (int) Page.Workspace;
            
            _authenticationController?.Dispose();
            
            Model.StartupPageModel = null;
            Model.AuthenticationPageModel = null;
        }

        public override void Dispose()
        {
            _authenticationController?.Dispose();
            _workspaceController?.Dispose();
            
            base.Dispose();
        }
    }
}