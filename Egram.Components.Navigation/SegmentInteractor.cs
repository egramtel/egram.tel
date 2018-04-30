using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Egram.Components.Navigation;
using Egram.Components.TDLib;
using ReactiveUI;

namespace Egram.Components.Navigation
{
    public class SegmentInteractor : IDisposable
    {
        private readonly IAgent _agent;

        public SegmentInteractor(
            IAgent agent
            )
        {
            _agent = agent;
        }

        public IObservable<Fetch> FetchAggregated()
        {
            return FetchAll().SelectMany(f =>
            {
                var fetches = new List<Fetch>(4);
                
                fetches.Add(new Fetch
                {
                    Segment = new Segment
                    {
                        Name = "Bots",
                        Kind = ExplorerEntityKind.Header | ExplorerEntityKind.Bot
                    },
                    Conversations = f.Conversations.Where(c => c.Kind == ExplorerEntityKind.Bot)
                        .Take(3)
                        .ToList()
                });

                fetches.Add(new Fetch
                {
                    Segment = new Segment
                    {
                        Name = "Channels",
                        Kind = ExplorerEntityKind.Header | ExplorerEntityKind.Channel
                    },
                    Conversations = f.Conversations.Where(c => c.Kind == ExplorerEntityKind.Channel)
                        .Take(3)
                        .ToList()
                });
                
                fetches.Add(new Fetch
                {
                    Segment = new Segment
                    {
                        Name = "Groups",
                        Kind = ExplorerEntityKind.Header | ExplorerEntityKind.Group
                    },
                    Conversations = f.Conversations.Where(c => c.Kind == ExplorerEntityKind.Group)
                        .Take(3)
                        .ToList()
                });
                
                fetches.Add(new Fetch
                {
                    Segment = new Segment
                    {
                        Name = "People",
                        Kind = ExplorerEntityKind.Header | ExplorerEntityKind.People
                    },
                    Conversations = f.Conversations.Where(c => c.Kind == ExplorerEntityKind.People)
                        .ToList()
                });
                
                return fetches;
            });
        }

        public IObservable<Fetch> FetchByKind(ExplorerEntityKind kind)
        {
            return FetchAll().Select(f => new Fetch
            {
                Conversations = f.Conversations.Where(t => t.Kind == kind).ToList()
            });
        }

        public IObservable<Fetch> FetchAll()
        {
            return Observable.Create<Fetch>(async observer =>
            {
                var conversations = new List<Conversation>();
                var chats = await GetAllChatsAsync();
                
                foreach (var chat in chats)
                {
                    switch (chat.Type)
                    {
                        case TD.ChatType.ChatTypePrivate ctp:
                            var user = await GetUserAsync(ctp.UserId);
                            if (user.Type is TD.UserType.UserTypeRegular)
                            {
                                conversations.Add(new Conversation
                                {
                                    Kind = ExplorerEntityKind.People,
                                    Topic = new Topic(chat)
                                });
                            }
                            else if (user.Type is TD.UserType.UserTypeBot)
                            {
                                conversations.Add(new Conversation
                                {
                                    Kind = ExplorerEntityKind.Bot,
                                    Topic = new Topic(chat)
                                });
                            }
                            break;
                        
                        case TD.ChatType.ChatTypeBasicGroup _:
                            conversations.Add(new Conversation
                            {
                                Kind = ExplorerEntityKind.Group,
                                Topic = new Topic(chat)
                            });
                            break;
                            
                        case TD.ChatType.ChatTypeSupergroup cts:
                            if (cts.IsChannel)
                            {
                                conversations.Add(new Conversation
                                {
                                    Kind = ExplorerEntityKind.Channel,
                                    Topic = new Topic(chat)
                                });
                            }
                            else
                            {
                                conversations.Add(new Conversation
                                {
                                    Kind = ExplorerEntityKind.Group,
                                    Topic = new Topic(chat)
                                });
                            }
                            break;
                    }
                }
                
                observer.OnNext(new Fetch
                {
                    Conversations = conversations
                });
            });
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
        
        public class Fetch
        {
            public Segment Segment { get; set; }
        
            public IList<Conversation> Conversations { get; set; }
        }
    }
}