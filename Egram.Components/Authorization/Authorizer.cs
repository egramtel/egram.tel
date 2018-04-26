using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Egram.Components.TDLib;

namespace Egram.Components.Authorization
{
    public class Authorizer
    {
        private readonly IAgent _agent;
        private readonly Parameters _parameters;

        public Authorizer(
            IAgent agent,
            Parameters parameters)
        {
            _agent = agent;
            _parameters = parameters;
        }

        public IObservable<TD.AuthorizationState> States()
        {
            return Observable.FromEventPattern<TD.Structure>(
                    h => _agent.Received += h,
                    h => _agent.Received -= h)
                .Select(s => s.EventArgs as TD.Update.UpdateAuthorizationState)
                .Where(u => u != null)
                .Select(u => u.AuthorizationState);
        }

        public Task SetupParameters()
        {
            return _agent.ExecuteAsync(
                new TD.SetTdlibParameters
                {
                    Parameters = new TD.TdlibParameters
                    {
                        UseTestDc = _parameters.UseTestDc,
                        DatabaseDirectory = _parameters.FilesDir,
                        FilesDirectory = _parameters.FilesDir,
                        UseFileDatabase = true,
                        UseChatInfoDatabase = true,
                        UseMessageDatabase = true,
                        UseSecretChats = true,
                        ApiId = _parameters.ApiId,
                        ApiHash = new Guid(_parameters.ApiHash).ToString("N"),
                        SystemLanguageCode = _parameters.Lang,
                        DeviceModel = _parameters.Device,
                        SystemVersion = _parameters.SystemVersion,
                        ApplicationVersion = _parameters.AppVersion,
                        EnableStorageOptimizer = true,
                        IgnoreFileNames = false
                    }
                });
        }

        public Task CheckEncryptionKey()
        {
            return _agent.ExecuteAsync(
                new TD.CheckDatabaseEncryptionKey
                {
                });
        }

        public Task SetPhoneNumber(string phoneNumber)
        {
            return _agent.ExecuteAsync(
                new TD.SetAuthenticationPhoneNumber
                {
                    PhoneNumber = phoneNumber
                });
        }

        public Task CheckCode(string code)
        {
            return _agent.ExecuteAsync(
                new TD.CheckAuthenticationCode
                {
                    Code = code
                });
        }

        public Task CheckCode(string code, string firstName, string lastName)
        {
            return _agent.ExecuteAsync(
                new TD.CheckAuthenticationCode
                {
                    Code = code,
                    FirstName = firstName,
                    LastName = lastName
                });
        }

        public Task CheckPassword(string password)
        {   
            return _agent.ExecuteAsync(
                new TD.CheckAuthenticationPassword
                {
                    Password = password
                });
        }
        
        public class Parameters
        {
            public bool UseTestDc { get; set; }
        
            public string FilesDir { get; set; }
        
            public int ApiId { get; set; }
        
            public byte[] ApiHash { get; set; }
        
            public string Lang { get; set; }
        
            public string Device { get; set; }
        
            public string SystemVersion { get; set; }
        
            public string AppVersion { get; set; }
        }
    }
}