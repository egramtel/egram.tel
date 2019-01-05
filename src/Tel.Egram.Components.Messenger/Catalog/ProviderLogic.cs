using System;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;
using Splat;
using Tel.Egram.Utils.Reactive;

namespace Tel.Egram.Components.Messenger.Catalog
{
    public static class ProviderLogic
    {
        public static IDisposable BindProvider(
            this CatalogModel model)
        {
            return BindProvider(
                model,
                Locator.Current.GetService<ICatalogProvider>());
        }

        public static IDisposable BindProvider(
            this CatalogModel model,
            ICatalogProvider provider)
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
                .Accept();
        }
    }
}