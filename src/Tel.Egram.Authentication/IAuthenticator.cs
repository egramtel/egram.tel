using System;
using System.Reactive;
using TdLib;

namespace Tel.Egram.Authentication
{
    public interface IAuthenticator
    {
        IObservable<TdApi.AuthorizationState> ObserveState();
        IObservable<TdApi.AuthorizationState> SetupParameters();
        IObservable<TdApi.AuthorizationState> CheckEncryptionKey();
        IObservable<TdApi.AuthorizationState> SetPhoneNumber(string phoneNumber);
        IObservable<TdApi.AuthorizationState> CheckCode(string code, string firstName = null, string lastName = null);
        IObservable<TdApi.AuthorizationState> CheckPassword(string password);
    }
}