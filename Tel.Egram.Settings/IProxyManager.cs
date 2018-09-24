using System;
using System.Reactive;
using TdLib;

namespace Tel.Egram.Settings
{
    public interface IProxyManager
    {
        IObservable<TdApi.Proxy[]> GetAllProxies();

        IObservable<TdApi.Proxy> AddProxy(TdApi.Proxy proxy);

        IObservable<TdApi.Proxy> UpdateProxy(TdApi.Proxy proxy);

        IObservable<Unit> RemoveProxy(TdApi.Proxy proxy);

        IObservable<Unit> EnableProxy(TdApi.Proxy proxy);

        IObservable<Unit> DisableProxy();
    }
}