using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Models.Messenger.Catalog;
using Tel.Egram.Models.Messenger.Catalog.Entries;

namespace Tel.Egram.Components.Messenger.Catalog
{
    public class CatalogController : BaseController<CatalogModel>
    {
        private readonly Subject<IComparer<EntryModel>> _sortingController;
        private readonly Subject<Func<EntryModel, bool>> _filterController;
        
        public CatalogController(Section section, ICatalogProvider catalogProvider)
        {
            _filterController = new Subject<Func<EntryModel, bool>>();
            _sortingController = new Subject<IComparer<EntryModel>>();

            BindProvider(catalogProvider).DisposeWith(this);
            BindSearch(section).DisposeWith(this);
        }

        private IDisposable BindProvider(ICatalogProvider catalogProvider)
        {
            var entries = Model.Entries;
            
            return catalogProvider.Chats.Connect()
                .Filter(_filterController)
                .Sort(_sortingController)
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(entries)
                .Subscribe();
        }

        private IDisposable BindSearch(Section section)
        {   
            return Model.WhenAnyValue(m => m.SearchText)
                .Throttle(TimeSpan.FromMilliseconds(500))
                .Subscribe(text =>
                {
                    var sorting = GetSorting(e => e.Order);
                    var filter = GetFilter(section);
                    
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        sorting = GetSorting(e => e.Title);
                        
                        filter = entry =>
                            entry.Title.Contains(text)
                                && GetFilter(section)(entry);
                    }
                    
                    _sortingController.OnNext(sorting);
                    _filterController.OnNext(filter);
                });
        }

        private Func<EntryModel, bool> GetFilter(Section section)
        {
            switch (section)
            {
                case Section.Bots:
                    return CatalogFilter.BotFilter;
                
                case Section.Channels:
                    return CatalogFilter.ChannelFilter;
                
                case Section.Groups:
                    return CatalogFilter.GroupFilter;
                
                case Section.Directs:
                    return CatalogFilter.DirectFilter;
                
                case Section.Home:
                default:
                    return CatalogFilter.All;
            }
        }

        private IComparer<EntryModel> GetSorting(Func<EntryModel, IComparable> f)
        {
            return SortExpressionComparer<EntryModel>.Ascending(f);
        }
    }
}