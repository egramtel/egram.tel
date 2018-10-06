using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Media;
using Avalonia.Threading;
using DynamicData;
using DynamicData.Alias;
using DynamicData.Binding;
using PropertyChanged;
using ReactiveUI;
using TdLib;
using Tel.Egram.Graphics;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Catalog
{
    [AddINotifyPropertyChangedInterface]
    public class CatalogContext : IDisposable
    {
        private readonly CompositeDisposable _contextDisposable = new CompositeDisposable();
        
        public EntryModel SelectedEntry { get; set; }
        
        public ObservableCollectionExtended<EntryModel> Entries { get; }
            = new ObservableCollectionExtended<EntryModel>();

        public CatalogContext(
            IFactory<ICatalogProvider> catalogProviderFactory,
            CatalogKind kind)
        {
            var catalogService = catalogProviderFactory.Create();
            BindCatalog(catalogService, kind).DisposeWith(_contextDisposable);
        }

        private IDisposable BindCatalog(ICatalogProvider catalogProvider, CatalogKind kind)
        {
            return catalogProvider.Chats.Connect()
                .Filter(GetFilter(kind))
                .Sort(GetSorting())
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(Entries)
                .Subscribe();
        }

        private Func<EntryModel, bool> GetFilter(CatalogKind kind)
        {
            switch (kind)
            {
                case CatalogKind.Bots:
                    return CatalogFilter.BotFilter;
                
                case CatalogKind.Channels:
                    return CatalogFilter.ChannelFilter;
                
                case CatalogKind.Groups:
                    return CatalogFilter.GroupFilter;
                
                case CatalogKind.Direct:
                    return CatalogFilter.DirectFilter;
                
                case CatalogKind.Home:
                default:
                    return CatalogFilter.All;
            }
        }

        private IComparer<EntryModel> GetSorting()
        {
            return SortExpressionComparer<EntryModel>.Ascending(p => p.Order);
        }

        public void Dispose()
        {
            _contextDisposable.Dispose();
        }
    }
}