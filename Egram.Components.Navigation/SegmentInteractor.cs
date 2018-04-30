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
            return Observable.Create<Fetch>(async observer =>
            {
                var botConversations = new ReactiveList<Conversation>();
                var channelConversations = new ReactiveList<Conversation>();
                var groupConversations = new ReactiveList<Conversation>();
                var peopleConversations = new ReactiveList<Conversation>();

                var chats = await GetAllChatsAsync();

                foreach (var chat in chats)
                {
                    switch (chat.Type)
                    {
                        case TD.ChatType.ChatTypePrivate ctp:
                            var user = await GetUserAsync(ctp.UserId);
                            if (user.Type is TD.UserType.UserTypeRegular)
                            {
                                peopleConversations.Add(new Conversation(ExplorerEntityKind.People, new Topic(chat)));
                            }
                            else if (user.Type is TD.UserType.UserTypeBot)
                            {
                                botConversations.Add(new Conversation(ExplorerEntityKind.Bot, new Topic(chat)));
                            }
                            break;
                        
                        case TD.ChatType.ChatTypeBasicGroup _:
                            groupConversations.Add(new Conversation(ExplorerEntityKind.Group, new Topic(chat)));
                            break;
                            
                        case TD.ChatType.ChatTypeSupergroup cts:
                            if (cts.IsChannel)
                            {
                                channelConversations.Add(new Conversation(ExplorerEntityKind.Channel, new Topic(chat)));
                            }
                            else
                            {
                                groupConversations.Add(new Conversation(ExplorerEntityKind.Group, new Topic(chat)));
                            }
                            break;
                    }
                }
                
                observer.OnNext(new Fetch
                {
                    Segment = new Segment("Bots", ExplorerEntityKind.Bot),
                    Conversations = botConversations.Take(3).ToList()
                });
                
                observer.OnNext(new Fetch
                {
                    Segment = new Segment("Channels", ExplorerEntityKind.Channel),
                    Conversations = channelConversations.Take(3).ToList()
                });
                
                observer.OnNext(new Fetch
                {
                    Segment = new Segment("Groups", ExplorerEntityKind.Group),
                    Conversations = groupConversations.Take(3).ToList()
                });
                
                observer.OnNext(new Fetch
                {
                    Segment = new Segment("People", ExplorerEntityKind.People),
                    Conversations = peopleConversations
                });
            });
        }

        public IObservable<Fetch> FetchByKind(ExplorerEntityKind kind)
        {
            return Observable.Create<Fetch>(async observer =>
            {
                var conversations = new ReactiveList<Conversation>();
                var chats = await GetAllChatsAsync();
                
                foreach (var chat in chats)
                {
                    switch (chat.Type)
                    {
                        case TD.ChatType.ChatTypePrivate ctp:
                            var user = await GetUserAsync(ctp.UserId);
                            if (user.Type is TD.UserType.UserTypeRegular)
                            {
                                conversations.Add(new Conversation(ExplorerEntityKind.People, new Topic(chat)));
                            }
                            else if (user.Type is TD.UserType.UserTypeBot)
                            {
                                conversations.Add(new Conversation(ExplorerEntityKind.Bot, new Topic(chat)));
                            }
                            break;
                        
                        case TD.ChatType.ChatTypeBasicGroup _:
                            conversations.Add(new Conversation(ExplorerEntityKind.Group, new Topic(chat)));
                            break;
                            
                        case TD.ChatType.ChatTypeSupergroup cts:
                            if (cts.IsChannel)
                            {
                                conversations.Add(new Conversation(ExplorerEntityKind.Channel, new Topic(chat)));
                            }
                            else
                            {
                                conversations.Add(new Conversation(ExplorerEntityKind.Group, new Topic(chat)));
                            }
                            break;
                    }
                }
                
                observer.OnNext(new Fetch
                {
                    Conversations = conversations.Where(t => t.Kind == kind).ToList()
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