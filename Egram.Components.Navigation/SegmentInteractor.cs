using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Egram.Components.Graphics;
using Egram.Components.I18N;
using Egram.Components.Navigation;
using Egram.Components.TDLib;
using ReactiveUI;

namespace Egram.Components.Navigation
{
    public class SegmentInteractor : IDisposable
    {
        private readonly IAgent _agent;
        private readonly ConversationLoader _conversationLoader;
        private readonly AvatarLoader _avatarLoader;

        public SegmentInteractor(
            IAgent agent,
            ConversationLoader conversationLoader,
            AvatarLoader avatarLoader
            )
        {
            _agent = agent;
            _conversationLoader = conversationLoader;
            _avatarLoader = avatarLoader;
        }

        public IObservable<Fetch> FetchAggregated()
        {
            return FetchAll().SelectMany(fetch =>
            {
                var results = new List<Fetch>();

                results.Add(new Fetch(
                    new Segment
                    {
                        Name = GetSegmentName(ExplorerEntityKind.Bot),
                        Kind = ExplorerEntityKind.Header | ExplorerEntityKind.Bot
                    },
                    fetch.Conversations.Where(c => c.Kind == ExplorerEntityKind.Bot)
                        .Take(4)
                        .ToList(),
                    fetch.Updates));

                results.Add(new Fetch(
                    new Segment
                    {
                        Name = GetSegmentName(ExplorerEntityKind.Channel),
                        Kind = ExplorerEntityKind.Header | ExplorerEntityKind.Channel
                    },
                    fetch.Conversations.Where(c => c.Kind == ExplorerEntityKind.Channel)
                        .Take(4)
                        .ToList(),
                    fetch.Updates));

                results.Add(new Fetch(
                    new Segment
                    {
                        Name = GetSegmentName(ExplorerEntityKind.Group),
                        Kind = ExplorerEntityKind.Header | ExplorerEntityKind.Group
                    },
                    fetch.Conversations.Where(c => c.Kind == ExplorerEntityKind.Group)
                        .Take(4)
                        .ToList(),
                    fetch.Updates));

                results.Add(new Fetch(
                    new Segment
                    {
                        Name = GetSegmentName(ExplorerEntityKind.People),
                        Kind = ExplorerEntityKind.Header | ExplorerEntityKind.People
                    },
                    fetch.Conversations.Where(c => c.Kind == ExplorerEntityKind.People)
                        .ToList(),
                    fetch.Updates));
                
                return results;
            });
        }

        public IObservable<Fetch> FetchByKind(ExplorerEntityKind kind)
        {
            return FetchAll().Select(fetch =>
                new Fetch(
                    new Segment
                    {
                        Name = GetSegmentName(kind),
                        Kind = ExplorerEntityKind.Header | kind
                    },
                    fetch.Conversations.Where(t => t.Kind == kind)
                        .ToList(),
                    fetch.Updates));
        }

        public IObservable<Fetch> FetchAll()
        {
            return GetAllChatsAsync()
                .ToObservable()
                .SelectMany(list => list)
                .SelectMany(chat =>
                {
                    switch (chat.Type)
                    {
                        case TD.ChatType.ChatTypePrivate ctp:
                            return GetUserAsync(ctp.UserId).ToObservable()
                                .Select(user =>
                                {
                                    _conversationLoader.Retrieve(chat, user, out var conversation);
                                    return conversation;
                                });

                        default:
                            return Observable.Range(0, 1)
                                .Select(_ =>
                                {
                                    _conversationLoader.Retrieve(chat, out var conversation);
                                    return conversation;
                                });
                    }
                })
                .Aggregate(new List<Conversation>(), (acc, c) =>
                {
                    acc.Add(c);
                    return acc;
                })
                .Select(conversations => new Fetch(conversations, LoadAvatars(conversations)));
        }

        private IObservable<Update> LoadAvatars(List<Conversation> conversations)
        {
            return Observable.Create<Update>(async observer =>
            {
                var nextToLoad = new List<Conversation>();
                
                // fast loading from disk
                foreach (var conversation in conversations)
                {
                    if (_avatarLoader.IsAvatarReady(conversation.Chat, AvatarLoader.Size.Explorer))
                    {
                        // load photo
                        var bitmap = await _avatarLoader.LoadForChatAsync(conversation.Chat, AvatarLoader.Size.Explorer);
                        observer.OnNext(new Update(conversation, bitmap));
                    }
                    else
                    {
                        // load fallback avatar
                        var bitmap = await _avatarLoader.LoadForChatAsync(conversation.Chat, AvatarLoader.Size.Explorer, true);
                        observer.OnNext(new Update(conversation, bitmap));
                        
                        nextToLoad.Add(conversation);
                    }
                }
                
                // slow loading (networking, processing, etc.)
                foreach (var conversation in nextToLoad)
                {
                    var bitmap = await _avatarLoader.LoadForChatAsync(conversation.Chat, AvatarLoader.Size.Explorer);
                    observer.OnNext(new Update(conversation, bitmap));
                }
            });
        }

        private Phrase GetSegmentName(ExplorerEntityKind kind)
        {
            switch (kind)
            {
                case ExplorerEntityKind.Bot:
                    return Phrase.Get("Bots");
                case ExplorerEntityKind.Channel:
                    return Phrase.Get("Channels");
                case ExplorerEntityKind.Group:
                    return Phrase.Get("Groups");
                case ExplorerEntityKind.People:
                    return Phrase.Get("People");
            }

            return null;
        }

        private async Task<TD.User> GetUserAsync(int userId)
        {
            var user = await _agent.ExecuteAsync(
                new TD.GetUser
                {
                    UserId = userId
                });

            return user;
        }

        private async Task<IList<TD.Chat>> GetAllChatsAsync()
        {
            var chatList = new List<TD.Chat>();

            long offsetChatId = 0;
            long offsetOrder = long.MaxValue;
            int count;

            do
            {
                var chats = await _agent.ExecuteAsync(
                    new TD.GetChats
                    {
                        OffsetChatId = offsetChatId,
                        OffsetOrder = offsetOrder,
                        Limit = 100,
                    });

                count = chats.ChatIds.Length;

                if (count > 0)
                {
                    foreach (var chatId in chats.ChatIds)
                    {
                        var chat = await GetChatAsync(chatId);
                        chatList.Add(chat);
                    }

                    offsetChatId = chatList.Last().Id;
                    offsetOrder = chatList.Last().Order;
                }

            } while (count > 0);

            return chatList;
        }

        private async Task<TD.Chat> GetChatAsync(long chatId)
        {
            var chat = await _agent.ExecuteAsync(
                new TD.GetChat
                {
                    ChatId = chatId
                });

            return chat;
        }

        public void Dispose()
        {
            
        }
        
        public class Update
        {
            public readonly Conversation Conversation;
            public readonly IBitmap Avatar;

            public Update(Conversation conversation, IBitmap avatar)
            {
                Conversation = conversation;
                Avatar = avatar;
            }
        }
        
        public class Fetch
        {
            public readonly Segment Segment;
            public readonly IList<Conversation> Conversations;
            public readonly IObservable<Update> Updates;

            public Fetch(Segment segment, IList<Conversation> conversations, IObservable<Update> updates)
            {
                Segment = segment;
                Conversations = conversations;
                Updates = updates;
            }

            public Fetch(IList<Conversation> conversations, IObservable<Update> updates)
            {
                Conversations = conversations;
                Updates = updates;
            }
        }
    }
}