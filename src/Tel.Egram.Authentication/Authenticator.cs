using System;
using System.Reactive;
using System.Reactive.Linq;
using TdLib;
using Tel.Egram.Persistance;
using Tel.Egram.Utils.TdLib;

namespace Tel.Egram.Authentication
{
    public class Authenticator : IAuthenticator
    {
        private readonly IAgent _agent;
        private readonly IStorage _storage;

        public Authenticator(
            IAgent agent,
            IStorage storage
            )
        {
            _agent = agent;
            _storage = storage;
        }

        public IObservable<TdApi.AuthorizationState> ObserveState()
        {   
            return _agent.Updates.OfType<TdApi.Update.UpdateAuthorizationState>()
                .Select(update => update.AuthorizationState);
        }
        
        public IObservable<TdApi.AuthorizationState> SetupParameters()
        {
            var state = ObserveState();
            
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
                })
                .SelectMany(_ => state)
                .FirstOrDefaultAsync();
        }

        public IObservable<TdApi.AuthorizationState> CheckEncryptionKey()
        {
            var state = ObserveState();
            
            return _agent.Execute(new TdApi.CheckDatabaseEncryptionKey())
                .SelectMany(_ => state)
                .FirstOrDefaultAsync();
        }

        public IObservable<TdApi.AuthorizationState> SetPhoneNumber(string phoneNumber)
        {
            var state = ObserveState();
            
            return _agent.Execute(new TdApi.SetAuthenticationPhoneNumber
                {
                    PhoneNumber = phoneNumber
                })
                .SelectMany(_ => state)
                .FirstOrDefaultAsync();
        }

        public IObservable<TdApi.AuthorizationState> CheckCode(string code, string firstName, string lastName)
        {
            var state = ObserveState();
            
            return _agent.Execute(new TdApi.CheckAuthenticationCode
                {
                    Code = code,
                    FirstName = firstName,
                    LastName = lastName
                })
                .SelectMany(_ => state)
                .FirstOrDefaultAsync();
        }

        public IObservable<TdApi.AuthorizationState> CheckPassword(string password)
        {
            var state = ObserveState();
            
            return _agent.Execute(new TdApi.CheckAuthenticationPassword
                {
                    Password = password
                })
                .SelectMany(_ => state)
                .FirstOrDefaultAsync();
        }
    }
}