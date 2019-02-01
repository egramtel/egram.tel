using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using DynamicData.Binding;
using ReactiveUI;
using TdLib;
using Tel.Egram.Model.Messenger.Catalog.Entries;
using Tel.Egram.Services.Messaging.Chats;
using Tel.Egram.Services.Utils.Reactive;

namespace Tel.Egram.Model.Messenger.Catalog
{
    public class CatalogFilter
    {
        public IDisposable Bind(
            CatalogModel model,
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
                    return BotFilter;
                
                case Section.Channels:
                    return ChannelFilter;
                
                case Section.Groups:
                    return GroupFilter;
                
                case Section.Directs:
                    return DirectFilter;
                
                case Section.Home:
                default:
                    return All;
            }
        }

        private static IComparer<EntryModel> GetSorting(Func<EntryModel, IComparable> f)
        {
            return SortExpressionComparer<EntryModel>.Ascending(f);
        }
        
        private static bool All(EntryModel model)
        {
            return true;
        }
        
        private static bool BotFilter(EntryModel model)
        {
            if (model is ChatEntryModel chatEntryModel)
            {
                var chat = chatEntryModel.Chat;
                if (chat.ChatData.Type is TdApi.ChatType.ChatTypePrivate)
                {
                    return chat.User != null &&
                           chat.User.Type is TdApi.UserType.UserTypeBot;
                }
            }
            return false;
        }

        private static bool DirectFilter(EntryModel model)
        {
            if (model is ChatEntryModel chatEntryModel)
            {
                var chat = chatEntryModel.Chat;
                if (chat.ChatData.Type is TdApi.ChatType.ChatTypePrivate)
                {
                    return chat.User != null &&
                           chat.User.Type is TdApi.UserType.UserTypeRegular;
                }
            }
            return false;
        }

        private static bool GroupFilter(EntryModel model)
        {
            if (model is ChatEntryModel chatEntryModel)
            {
                var chat = chatEntryModel.Chat;
                if (chat.ChatData.Type is TdApi.ChatType.ChatTypeSupergroup supergroupType)
                {
                    return !supergroupType.IsChannel;
                }

                return chat.ChatData.Type is TdApi.ChatType.ChatTypeBasicGroup;
            }
            return false;
        }

        private static bool ChannelFilter(EntryModel model)
        {
            if (model is ChatEntryModel chatEntryModel)
            {
                var chat = chatEntryModel.Chat;
                if (chat.ChatData.Type is TdApi.ChatType.ChatTypeSupergroup supergroupType)
                {
                    return supergroupType.IsChannel;
                }
            }
            return false;
        }
    }
}