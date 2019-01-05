using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using DynamicData.Binding;
using ReactiveUI;
using Tel.Egram.Components.Messenger.Catalog.Entries;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Utils.Reactive;

namespace Tel.Egram.Components.Messenger.Catalog
{
    public static class FilterLogic
    {
        public static IDisposable BindFilter(
            this CatalogModel model,
            Section section)
        {   
            return model.WhenAnyValue(m => m.SearchText)
                .Throttle(TimeSpan.FromMilliseconds(500))
                .Accept(text =>
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
                    
                    model.SortingController.OnNext(sorting);
                    model.FilterController.OnNext(filter);
                });
        }

        private static Func<EntryModel, bool> GetFilter(Section section)
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

        private static IComparer<EntryModel> GetSorting(Func<EntryModel, IComparable> f)
        {
            return SortExpressionComparer<EntryModel>.Ascending(f);
        }
    }
}