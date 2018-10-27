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
using Tel.Egram.Gui.Views.Authentication;
using Tel.Egram.Gui.Views.Workspace;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Application
{
    public class ApplicationController
        : BaseController, IApplicationController
    {   
        private readonly IFactory<AuthenticationPageModel, IAuthenticationController> _authenticationControllerFactory;
        private IAuthenticationController _authenticationController;
        
        private readonly IFactory<WorkspacePageModel, IWorkspaceController> _workspaceControllerFactory;
        private IWorkspaceController _workspaceController;

        public ApplicationController(
            MainWindowModel model,
            IAuthenticator authenticator,
            IApplicationPopupController applicationPopupController,
            IFactory<AuthenticationPageModel, IAuthenticationController> authenticationControllerFactory,
            IFactory<WorkspacePageModel, IWorkspaceController> workspaceControllerFactory)
        {
            _authenticationControllerFactory = authenticationControllerFactory;
            _workspaceControllerFactory = workspaceControllerFactory;
            
            BindAuthenticator(model, authenticator)
                .DisposeWith(this);

            BindPopup(model, applicationPopupController)
                .DisposeWith(this);
        }

        private IDisposable BindAuthenticator(
            MainWindowModel model,
            IAuthenticator authenticator)
        {
            return authenticator
                .ObserveState()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(state =>
                {   
                    switch (state)
                    {
                        case TdApi.AuthorizationState.AuthorizationStateWaitTdlibParameters _:
                            GoToStartupPage(model);
                            authenticator.SetupParameters()
                                .Subscribe()
                                .DisposeWith(this);
                            break;
                    
                        case TdApi.AuthorizationState.AuthorizationStateWaitEncryptionKey _:
                            GoToStartupPage(model);
                            authenticator.CheckEncryptionKey()
                                .Subscribe()
                                .DisposeWith(this);
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
                });
        }

        private IDisposable BindPopup(
            MainWindowModel model,
            IApplicationPopupController controller)
        {
            return Observable.FromEventPattern<PopupControlModel>(
                    h => controller.ContextChanged += h,
                    h => controller.ContextChanged -= h)
                .Select(e => e.EventArgs)
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(popupModel =>
                {
                    model.PopupControlModel = popupModel ?? new PopupControlModel
                    {
                        IsPopupVisible = false
                    };
                });
        }

        private void GoToStartupPage(MainWindowModel model)
        {
            if (model.StartupPageModel == null)
            {
                var startupPageModel = new StartupPageModel();
                model.StartupPageModel = startupPageModel;
            }
            
            model.PageIndex = (int) Page.Initial;

            _authenticationController?.Dispose();
            _workspaceController?.Dispose();
            
            model.WorkspacePageModel = null;
            model.AuthenticationPageModel = null;
        }

        private void GoToAuthenticationPage(MainWindowModel model)
        {
            if (model.AuthenticationPageModel == null)
            {
                var authenticationPageModel = new AuthenticationPageModel();
                _authenticationController = _authenticationControllerFactory.Create(authenticationPageModel);
                model.AuthenticationPageModel = authenticationPageModel;
            }
            
            model.PageIndex = (int) Page.Authentication;

            _workspaceController?.Dispose();

            model.StartupPageModel = null;
            model.WorkspacePageModel = null;
        }

        private void GoToWorkspacePage(MainWindowModel model)
        {
            if (model.WorkspacePageModel == null)
            {
                var workspacePageModel = new WorkspacePageModel();
                _workspaceController = _workspaceControllerFactory.Create(workspacePageModel);
                model.WorkspacePageModel = workspacePageModel;
            }
            
            model.PageIndex = (int) Page.Workspace;
            
            _authenticationController?.Dispose();
            
            model.StartupPageModel = null;
            model.AuthenticationPageModel = null;
        }

        public override void Dispose()
        {
            _authenticationController?.Dispose();
            _workspaceController?.Dispose();
            
            base.Dispose();
        }
    }
}