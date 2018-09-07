using System;
using System.Reactive.Linq;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using TdLib;
using Tel.Egram.Persistance;
using Tel.Egram.TdLib;

namespace Tel.Egram.Components.Application
{
    public class AuthenticationInteractor : IDisposable
    {
        private readonly IStorage _storage;
        private readonly TdAgent _agent;

        public AuthenticationInteractor(
            IStorage storage,
            TdAgent agent
            )
        {
            _storage = storage;
            _agent = agent;
        }
        
        public IDisposable Bind(ApplicationContext context)
        {
            return _agent.Updates.OfType<TdApi.Update.UpdateAuthorizationState>()
                .Select(update => update.AuthorizationState)
                .ObserveOn(AvaloniaScheduler.Instance)
                .Subscribe(state =>
                {
                    switch (state)
                    {
                        case TdApi.AuthorizationState.AuthorizationStateWaitTdlibParameters _:
                            context.InitLoading();
                            SetupParameters();
                            break;
                    
                        case TdApi.AuthorizationState.AuthorizationStateWaitEncryptionKey _:
                            context.InitLoading();
                            CheckEncryptionKey();
                            break;
                    
                        case TdApi.AuthorizationState.AuthorizationStateWaitPhoneNumber _:
                        case TdApi.AuthorizationState.AuthorizationStateWaitCode _:
                        case TdApi.AuthorizationState.AuthorizationStateWaitPassword _:
                            context.InitAuthentication();
                            break;
                
                        case TdApi.AuthorizationState.AuthorizationStateReady _:
                            context.InitWorkspace();
                            break;
                    }
                });
        }

        private IObservable<TdApi.Ok> SetupParameters()
        {   
            return _agent.Execute(new TdApi.SetTdlibParameters
            {
                Parameters = new TdApi.TdlibParameters
                {
                    UseTestDc = false,
                    DatabaseDirectory = _storage.TdLibDirectory,
                    FilesDirectory = _storage.TdLibDirectory,
                    UseFileDatabase = true,
                    UseChatInfoDatabase = true,
                    UseMessageDatabase = true,
                    UseSecretChats = true,
                    ApiId = 111112,
                    ApiHash = new Guid(new byte[]{ 142, 34, 97, 121, 94, 51, 206, 139, 4, 159, 245, 26, 236, 242, 11, 171 }).ToString("N"),
                    SystemLanguageCode = "en",
                    DeviceModel = "Mac",
                    SystemVersion = "0.1",
                    ApplicationVersion = "0.1",
                    EnableStorageOptimizer = true,
                    IgnoreFileNames = false
                }
            });
        }

        private IObservable<TdApi.Ok> CheckEncryptionKey()
        {
            return _agent.Execute(new TdApi.CheckDatabaseEncryptionKey());
        }

        public void Dispose()
        {
            
        }
    }
}