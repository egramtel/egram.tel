using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;
using Splat;
using Tel.Egram.Model.Messenger.Catalog.Entries;
using Tel.Egram.Services.Messaging.Chats;
using Tel.Egram.Services.Utils.Reactive;

namespace Tel.Egram.Model.Messenger.Catalog
{
    public class CatalogProvider
    {
        private readonly IChatLoader _chatLoader;
        private readonly IChatUpdater _chatUpdater;

        private readonly Dictionary<long, EntryModel> _entryStore;
        private readonly SourceCache<EntryModel, long> _chats;

        public CatalogProvider(
            IChatLoader chatLoader,
            IChatUpdater chatUpdater
            )
        {
            _chatLoader = chatLoader;
            _chatUpdater = chatUpdater;
            
            _entryStore = new Dictionary<long, EntryModel>();
            _chats = new SourceCache<EntryModel, long>(m => m.Id);
        }

        public CatalogProvider()
            : this(
                Locator.Current.GetService<IChatLoader>(),
                Locator.Current.GetService<IChatUpdater>())
        {
        }

        public IDisposable Bind(CatalogModel model, Section section)
        {
            var entries = model.Entries;
            var filter = model.FilterController;
            var sorting = model.SortingController;
            
            var disposable = new CompositeDisposable();

            LoadHome(model, section)
                .DisposeWith(disposable);
            
            _chats.Connect()
                .Filter(filter)
                .Sort(sorting)
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(entries)
                .Accept()
                .DisposeWith(disposable);
            
            BindOrderUpdates()
                .DisposeWith(disposable);
            
            BindEntryUpdates()
                .DisposeWith(disposable);
            
            LoadChats()
                .DisposeWith(disposable);

            return disposable;
        }

        /// <summary>
        /// Load home
        /// </summary>
        private IDisposable LoadHome(CatalogModel model, Section section)
        {
            if (section == Section.Home)
            {
                model.Entries.Add(HomeEntryModel.Instance);
                model.SelectedEntry = HomeEntryModel.Instance;
            
                _chats.AddOrUpdate(HomeEntryModel.Instance);
            }
            
            return Disposable.Empty;
        }

        /// <summary>
        /// Load chats into observable cache
        /// </summary>
        private IDisposable LoadChats()
        {
            return _chatLoader.LoadChats()
                .Select(GetChatEntryModel)
                .Aggregate(new List<EntryModel>(), (list, model) =>
                {
                    model.Order = list.Count;
                    list.Add(model);
                    return list;
                })
                .Accept(entries =>
                {
                    entries.Insert(0, HomeEntryModel.Instance);
                    
                    _chats.EditDiff(entries, (m1, m2) => m1.Id == m2.Id);
                    _chats.Refresh();
                });
        }

        /// <summary>
        /// Subscribe to updates that involve order change
        /// </summary>
        private IDisposable BindOrderUpdates()
        {
            return _chatUpdater.GetOrderUpdates()
                .Buffer(TimeSpan.FromSeconds(2))
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Where(changes => changes.Count > 0)
                .SelectMany(_ => _chatLoader.LoadChats()
                    .Select(GetChatEntryModel)
                    .ToList())
                .Do(entries =>
                {
                    for (int i = 0; i < entries.Count; i++)
                    {
                        entries[i].Order = i;
                    }
                })
                .Accept(entries =>
                {
                    entries.Insert(0, HomeEntryModel.Instance);
                    
                    _chats.EditDiff(entries, (m1, m2) => m1.Id == m2.Id);
                    _chats.Refresh();
                });
        }

        /// <summary>
        /// Subscribe to updates for individual entries
        /// </summary>
        private IDisposable BindEntryUpdates()
        {
            return _chatUpdater.GetChatUpdates()
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
    }
}