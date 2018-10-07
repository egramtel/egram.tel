using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
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
        
        private readonly Subject<IComparer<EntryModel>> _sortingController;
        private readonly Subject<Func<EntryModel, bool>> _filterController;
        
        public EntryModel SelectedEntry { get; set; }
        public ObservableCollectionExtended<EntryModel> Entries { get; set; }
        
        public string SearchText { get; set; }

        public CatalogContext(
            IFactory<ICatalogProvider> catalogProviderFactory,
            CatalogKind kind)
        {
            _filterController = new Subject<Func<EntryModel, bool>>();
            _sortingController = new Subject<IComparer<EntryModel>>();
            
            var catalogService = catalogProviderFactory.Create();
            BindCatalog(catalogService, kind).DisposeWith(_contextDisposable);

            BindSearch(kind).DisposeWith(_contextDisposable);
        }

        private IDisposable BindCatalog(ICatalogProvider catalogProvider, CatalogKind kind)
        {
            Entries = new ObservableCollectionExtended<EntryModel>();
            
            return catalogProvider.Chats.Connect()
                .Filter(_filterController)
                .Sort(_sortingController)
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(Entries)
                .Subscribe();
        }

        private IDisposable BindSearch(CatalogKind kind)
        {
            return this.WhenAnyValue(ctx => ctx.SearchText)
                .Throttle(TimeSpan.FromMilliseconds(500))
                .Subscribe(text =>
                {
                    var sorting = GetSorting(e => e.Order);
                    var filter = GetFilter(kind);
                    
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        sorting = GetSorting(e => e.Title);
                        
                        filter = entry =>
                            entry.Title.Contains(text) && GetFilter(kind)(entry);
                    }
                    
                    _sortingController.OnNext(sorting);
                    _filterController.OnNext(filter);
                });
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

        private IComparer<EntryModel> GetSorting(Func<EntryModel, IComparable> f)
        {
            return SortExpressionComparer<EntryModel>.Ascending(f);
        }

        public void Dispose()
        {
            _contextDisposable.Dispose();
        }
    }
}