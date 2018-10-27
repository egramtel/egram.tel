using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using Tel.Egram.Gui.Views.Messenger.Catalog;
using Tel.Egram.Gui.Views.Messenger.Catalog.Entries;
using Tel.Egram.Messaging.Chats;

namespace Tel.Egram.Components.Messenger.Catalog
{
    public class CatalogController
        : BaseController, ICatalogController
    {
        private readonly Subject<IComparer<EntryModel>> _sortingController;
        private readonly Subject<Func<EntryModel, bool>> _filterController;
        
        public CatalogController(
            CatalogControlModel model,
            ICatalogProvider catalogProvider)
        {
            _filterController = new Subject<Func<EntryModel, bool>>();
            _sortingController = new Subject<IComparer<EntryModel>>();

            BindProvider(model, catalogProvider)
                .DisposeWith(this);
            
            BindSearch(model)
                .DisposeWith(this);
        }

        private IDisposable BindProvider(
            CatalogControlModel model,
            ICatalogProvider catalogProvider)
        {   
            return catalogProvider.Chats.Connect()
                .Filter(_filterController)
                .Sort(_sortingController)
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(model.Entries)
                .Subscribe();
        }

        private IDisposable BindSearch(
            CatalogControlModel model)
        {
            var section = model.Section;
            
            return model.WhenAnyValue(m => m.SearchText)
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