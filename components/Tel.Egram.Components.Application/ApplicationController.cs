using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using TdLib;
using Tel.Egram.Authentication;
using Tel.Egram.Components.Authentication;
using Tel.Egram.Components.Workspace;
using Tel.Egram.Models.Application;
using Tel.Egram.Models.Application.Popup;
using Tel.Egram.Models.Application.Startup;
using Tel.Egram.Models.Authentication;
using Tel.Egram.Models.Workspace;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Application
{
    public class ApplicationController : Controller<MainWindowModel>
    {
        private IController<AuthenticationModel> _authenticationController;
        private IController<WorkspaceModel> _workspaceController;

        public ApplicationController(
            ISchedulers schedulers,
            IAuthenticator authenticator,
            IApplicationPopupController applicationPopupController)
        {   
            BindAuthenticator(schedulers, authenticator)
                .DisposeWith(this);

            BindPopup(schedulers, applicationPopupController)
                .DisposeWith(this);
        }

        private IDisposable BindAuthenticator(
            ISchedulers schedulers,
            IAuthenticator authenticator)
        {
            return authenticator
                .ObserveState()
                .ObserveOn(schedulers.Main)
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

        private IDisposable BindPopup(
            ISchedulers schedulers,
            IApplicationPopupController controller)
        {
            return Observable.FromEventPattern<PopupModel>(
                    h => controller.ContextChanged += h,
                    h => controller.ContextChanged -= h)
                .Select(e => e.EventArgs)
                .SubscribeOn(schedulers.Pool)
                .ObserveOn(schedulers.Main)
                .Subscribe(popupModel =>
                {
                    Model.PopupModel = popupModel ?? new PopupModel
                    {
                        IsPopupVisible = false
                    };
                });
        }

        private void GoToStartupPage()
        {
            if (Model.StartupModel == null)
            {
                var startupPageModel = new StartupModel();
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
                var model = Activate(ref _authenticationController);
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
                var model = Activate(ref _workspaceController);
                Model.WorkspaceModel = model;
            }
            
            Model.PageIndex = (int) Page.Workspace;
            
            _authenticationController?.Dispose();
            
            Model.StartupModel = null;
            Model.AuthenticationModel = null;
        }
    }
}