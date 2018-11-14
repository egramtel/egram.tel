using System;
using System.Reactive;
using TdLib;

namespace Tel.Egram.Authentication
{
    public interface IAuthenticator
    {
        IObservable<TdApi.AuthorizationState> ObserveState();
        IObservable<TdApi.Ok> SetupParameters();
        IObservable<TdApi.Ok> CheckEncryptionKey();
        IObservable<TdApi.Ok> SetPhoneNumber(string phoneNumber);
        IObservable<TdApi.Ok> CheckCode(string code, string firstName = null, string lastName = null);
        IObservable<TdApi.Ok> CheckPassword(string password);
    }
}