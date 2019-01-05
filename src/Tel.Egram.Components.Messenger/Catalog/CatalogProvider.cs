using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;
using TdLib;
using Tel.Egram.Components.Messenger.Catalog.Entries;
using Tel.Egram.Graphics;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Utils;
using Tel.Egram.Utils.Reactive;

namespace Tel.Egram.Components.Messenger.Catalog
{
    public class CatalogProvider : ICatalogProvider, IDisposable
    {
        private readonly CompositeDisposable _serviceDisposable = new CompositeDisposable();

        private readonly Dictionary<long, EntryModel> _entryStore;
        private readonly SourceCache<EntryModel, long> _chats;
        public IObservableCache<EntryModel, long> Chats => _chats;

        public CatalogProvider(
            IChatLoader chatLoader,
            IChatUpdater chatUpdater
            )
        {
            _entryStore = new Dictionary<long, EntryModel>();
            _chats = new SourceCache<EntryModel, long>(m => m.Id);
            
            LoadChats(chatLoader)
                .DisposeWith(_serviceDisposable);
            
            BindOrderUpdates(chatLoader, chatUpdater)
                .DisposeWith(_serviceDisposable);
            
            BindEntryUpdates(chatLoader, chatUpdater)
                .DisposeWith(_serviceDisposable);
        }

        /// <summary>
        /// Load chats into observable cache
        /// </summary>
        private IDisposable LoadChats(
            IChatLoader chatLoader)
        {
            return chatLoader.LoadChats()
                .Select(GetChatEntryModel)
                .Aggregate(new List<EntryModel>(), (list, model) =>
                {
                    model.Order = list.Count;
                    list.Add(model);
                    return list;
                })
                .Synchronize(_chats)
                .Accept(entries =>
                {
                    _chats.EditDiff(entries, (m1, m2) => m1.Id == m2.Id);
                    _chats.Refresh();
                });
        }

        /// <summary>
        /// Subscribe to updates that involve order change
        /// </summary>
        private IDisposable BindOrderUpdates(
            IChatLoader chatLoader,
            IChatUpdater chatUpdater)
        {
            return chatUpdater.GetOrderUpdates()
                .Buffer(TimeSpan.FromSeconds(1))
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Accept(changes =>
                {
                    if (changes.Count > 0)
                    {
                        LoadChats(chatLoader)
                            .DisposeWith(_serviceDisposable);
                    }
                });
        }

        /// <summary>
        /// Subscribe to updates for individual entries
        /// </summary>
        private IDisposable BindEntryUpdates(
            IChatLoader chatLoader,
            IChatUpdater chatUpdater)
        {
            return chatUpdater.GetChatUpdates()
                .Buffer(TimeSpan.FromSeconds(1))
                .SelectMany(chats => chats)
                .Select(chat => new
                    {
                        Chat = chat,
                        Entry = GetChatEntryModel(chat)
                    })
                .Accept(item =>
                {
                    UpdateChatEntryModel((ChatEntryModel)item.Entry, item.Chat);
                });
        }

        private EntryModel GetChatEntryModel(Chat chat)
        {
            var chatData = chat.ChatData;
            
            if (!_entryStore.TryGetValue(chatData.Id, out var entry))
            {
                entry = new ChatEntryModel
                {
                    Chat = chat
                };
                UpdateChatEntryModel((ChatEntryModel)entry, chat);
                
                _entryStore.Add(chatData.Id, entry);
            }

            return entry;
        }

        private void UpdateChatEntryModel(ChatEntryModel entry, Chat chat)
        {
            var chatData = chat.ChatData;
            
            entry.Chat = chat;
            entry.Id = chatData.Id;
            entry.Title = chatData.Title;
            entry.HasUnread = chatData.UnreadCount > 0;
            entry.UnreadCount = chatData.UnreadCount.ToString();
        }

        public void Dispose()
        {
            _serviceDisposable.Dispose();
        }
    }
}