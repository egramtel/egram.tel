using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;
using TdLib;
using Tel.Egram.Feeds;

namespace Tel.Egram.Components.Catalog
{
    public class CatalogProvider : ICatalogProvider
    {
        private readonly CompositeDisposable _serviceDisposable = new CompositeDisposable();

        private readonly Dictionary<long, ChatEntryModel> _entryStore;
        private readonly SourceCache<EntryModel, long> _chats;
        public IObservableCache<EntryModel, long> Chats => _chats;

        public CatalogProvider(
            IChatLoader chatLoader,
            IChatUpdater chatUpdater
            )
        {
            _entryStore = new Dictionary<long, ChatEntryModel>();
            _chats = new SourceCache<EntryModel, long>(m => m.Id);
            
            LoadChats(chatLoader).DisposeWith(_serviceDisposable);
            BindOrderUpdates(chatLoader, chatUpdater).DisposeWith(_serviceDisposable);
        }

        public bool BotFilter(EntryModel model)
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

        public bool DirectFilter(EntryModel model)
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

        public bool GroupFilter(EntryModel model)
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

        public bool ChannelFilter(EntryModel model)
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

        private IDisposable LoadChats(IChatLoader chatLoader)
        {
            return chatLoader.LoadChats()
                .Select(GetChatEntryModel)
                .Aggregate(new List<ChatEntryModel>(), (list, model) =>
                {
                    model.Order = list.Count;
                    list.Add(model);
                    return list;
                })
                .SubscribeOn(TaskPoolScheduler.Default)
                .Subscribe(models =>
                {
                    _chats.EditDiff(models, (m1, m2) => m1.Id == m2.Id);
                    _chats.Refresh();
                });
        }

        private IDisposable BindOrderUpdates(IChatLoader chatLoader, IChatUpdater chatUpdater)
        {
            return chatUpdater.GetOrderChanges()
                .Buffer(TimeSpan.FromSeconds(1))
                .SubscribeOn(TaskPoolScheduler.Default)
                .Subscribe(changes =>
                {
                    if (changes.Count > 0)
                    {
                        LoadChats(chatLoader).DisposeWith(_serviceDisposable);
                    }
                });
        }

        private ChatEntryModel GetChatEntryModel(Chat chat)
        {
            if (!_entryStore.TryGetValue(chat.ChatData.Id, out var entry))
            {
                entry = ChatEntryModel.FromChat(chat);
                _entryStore.Add(chat.ChatData.Id, entry);
            }

            return entry;
        }
    }
}