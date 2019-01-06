using System;
using System.Reactive.Linq;
using ReactiveUI;
using Splat;
using TdLib;
using Tel.Egram.Services.Utils.Reactive;
using Tel.Egram.Services.Utils.TdLib;

namespace Tel.Egram.Model.Application
{
    public static class ConnectionLogic
    {
        public static IDisposable BindConnectionInfo(
            this MainWindowModel model)
        {
            return BindConnectionInfo(
                model,
                Locator.Current.GetService<IAgent>());
        }

        public static IDisposable BindConnectionInfo(
            this MainWindowModel model,
            IAgent agent)
        {
            return agent.Updates.OfType<TdApi.Update.UpdateConnectionState>()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Accept(update => 
                {
                    switch (update.State)
                    {
                        case TdApi.ConnectionState.ConnectionStateConnecting _:
                            UpdateConnectionState(model, ConnectionState.Connecting);
                            break;
                        case TdApi.ConnectionState.ConnectionStateConnectingToProxy _:
                            UpdateConnectionState(model, ConnectionState.ConnectingToProxy);
                            break;
                        case TdApi.ConnectionState.ConnectionStateReady _:
                            UpdateConnectionState(model, ConnectionState.Ready);
                            break;
                        case TdApi.ConnectionState.ConnectionStateUpdating _:
                            UpdateConnectionState(model, ConnectionState.Updating);
                            break;
                        case TdApi.ConnectionState.ConnectionStateWaitingForNetwork _:
                            UpdateConnectionState(model, ConnectionState.WaitingForNetwork);
                            break;
                    }
                });
        }
        
        private static void UpdateConnectionState(MainWindowModel model, ConnectionState state)
        {
            string[] stateTexts = {
                "Connecting...",
                "Connecting to proxy...",
                "Ready.",
                "Updating...",
                "Waiting for network..."
            };

            model.ConnectionState = stateTexts[(int)state];
        }
    }
}