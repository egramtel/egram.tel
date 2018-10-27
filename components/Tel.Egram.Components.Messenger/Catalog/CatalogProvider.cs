using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;
using TdLib;
using Tel.Egram.Graphics;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Models.Messenger.Catalog.Entries;

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
            IChatUpdater chatUpdater,
            IAvatarLoader avatarLoader
            )
        {
            _entryStore = new Dictionary<long, EntryModel>();
            _chats = new SourceCache<EntryModel, long>(m => m.Id);
            
            LoadChats(chatLoader, avatarLoader)
                .DisposeWith(_serviceDisposable);
            BindOrderUpdates(chatLoader, chatUpdater, avatarLoader)
                .DisposeWith(_serviceDisposable);
            BindEntryUpdates(chatLoader, chatUpdater, avatarLoader)
                .DisposeWith(_serviceDisposable);
        }

        /// <summary>
        /// Load chats into observable cache
        /// </summary>
        private IDisposable LoadChats(IChatLoader chatLoader, IAvatarLoader avatarLoader)
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
                .Do(entries =>
                {
                    _chats.EditDiff(entries, (m1, m2) => m1.Id == m2.Id);
                    _chats.Refresh();
                })
                .SelectMany(entries => entries)
                .SelectMany(entry => LoadAvatar(avatarLoader, entry)
                    .Select(avatar => new
                    {
                        Entry = entry,
                        Avatar = avatar
                    }))
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(item =>
                {
                    var entry = item.Entry;
                    var avatar = item.Avatar;
                    entry.Avatar = avatar;
                });
        }

        /// <summary>
        /// Subscribe to updates that involve order change
        /// </summary>
        private IDisposable BindOrderUpdates(
            IChatLoader chatLoader,
            IChatUpdater chatUpdater,
            IAvatarLoader avatarLoader
            )
        {
            return chatUpdater.GetOrderUpdates()
                .Buffer(TimeSpan.FromSeconds(1))
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(changes =>
                {
                    if (changes.Count > 0)
                    {
                        LoadChats(chatLoader, avatarLoader).DisposeWith(_serviceDisposable);
                    }
                });
        }

        /// <summary>
        /// Subscribe to updates for individual entries
        /// </summary>
        private IDisposable BindEntryUpdates(
            IChatLoader chatLoader,
            IChatUpdater chatUpdater,
            IAvatarLoader avatarLoader
            )
        {
            return chatUpdater.GetChatUpdates()
                .Buffer(TimeSpan.FromSeconds(1))
                .SelectMany(chats => chats)
                .Select(chat => new
                    {
                        Chat = chat,
                        Entry = GetChatEntryModel(chat)
                    })
                .SelectMany(item => LoadAvatar(avatarLoader, item.Entry)
                    .Select(avatar => new
                    {
                        Chat = item.Chat,
                        Entry = item.Entry,
                        Avatar = avatar
                    }))
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(item =>
                {
                    UpdateChatEntryModel(item.Entry, item.Chat, item.Avatar);
                });
        }

        private IObservable<Avatar> LoadAvatar(IAvatarLoader avatarLoader, EntryModel entry)
        {
            if (entry.Avatar != null)
            {
                return Observable.Return(entry.Avatar);
            }
            
            switch (entry.Target)
            {
                case Chat chat:
                    return avatarLoader.LoadAvatar(chat.ChatData);
                
                case Aggregate aggregate:
                    return avatarLoader.LoadAvatar(new TdApi.Chat
                        {
                            Id = aggregate.Id
                        });
            }
            
            return Observable.Return<Avatar>(null);
        }

        private EntryModel GetChatEntryModel(Chat chat)
        {
            var chatData = chat.ChatData;
            
            if (!_entryStore.TryGetValue(chatData.Id, out var entry))
            {
                entry = EntryModel.FromTarget(chat);
                UpdateChatEntryModel(entry, chat);
                
                _entryStore.Add(chatData.Id, entry);
            }

            return entry;
        }

        private void UpdateChatEntryModel(EntryModel entry, Chat chat, Avatar avatar = null)
        {
            var chatData = chat.ChatData;
            
            entry.Target = chat;
            entry.Id = chatData.Id;
            entry.Title = chatData.Title;
            entry.Avatar = avatar;
            entry.HasUnread = chatData.UnreadCount > 0;
            entry.UnreadCount = chatData.UnreadCount.ToString();
        }

        public void Dispose()
        {
            _serviceDisposable.Dispose();
        }
    }
}