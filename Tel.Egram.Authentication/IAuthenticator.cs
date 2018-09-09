using System;
using System.Reactive;
using TdLib;

namespace Tel.Egram.Authentication
{
    public interface IAuthenticator
    {
        IObservable<TdApi.AuthorizationState> ObserveState();
        IObservable<Unit> SetupParameters();
        IObservable<Unit> CheckEncryptionKey();
        IObservable<Unit> SetPhoneNumber(string phoneNumber);
        IObservable<Unit> CheckCode(string code, string firstName, string lastName);
        IObservable<Unit> CheckPassword(string password);
    }
}