using System;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;

namespace Tel.Egram.Components.Messenger.Catalog
{
    public static class ProviderLogic
    {
        public static IDisposable BindProvider(
            this CatalogModel model,
            ICatalogProvider provider = null)
        {
            var entries = model.Entries;
            var filter = model.FilterController;
            var sorting = model.SortingController;
            
            return provider.Chats.Connect()
                .Filter(filter)
                .Sort(sorting)
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(entries)
                .Subscribe();
        }
    }
}