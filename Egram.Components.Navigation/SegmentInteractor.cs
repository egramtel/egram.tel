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
                var botTargets = new ReactiveList<SegmentTarget>();
                var channelTargets = new ReactiveList<SegmentTarget>();
                var groupTargets = new ReactiveList<SegmentTarget>();
                var peopleTargets = new ReactiveList<SegmentTarget>();

                var chats = await GetAllChatsAsync();

                foreach (var chat in chats)
                {
                    switch (chat.Type)
                    {
                        case TD.ChatType.ChatTypePrivate ctp:
                            var user = await GetUserAsync(ctp.UserId);
                            if (user.Type is TD.UserType.UserTypeRegular)
                            {
                                peopleTargets.Add(new SegmentTarget(ExplorerEntityKind.People, new Topic(chat)));
                            }
                            else if (user.Type is TD.UserType.UserTypeBot)
                            {
                                botTargets.Add(new SegmentTarget(ExplorerEntityKind.Bot, new Topic(chat)));
                            }
                            break;
                        
                        case TD.ChatType.ChatTypeBasicGroup _:
                            groupTargets.Add(new SegmentTarget(ExplorerEntityKind.Group, new Topic(chat)));
                            break;
                            
                        case TD.ChatType.ChatTypeSupergroup cts:
                            if (cts.IsChannel)
                            {
                                channelTargets.Add(new SegmentTarget(ExplorerEntityKind.Channel, new Topic(chat)));
                            }
                            else
                            {
                                groupTargets.Add(new SegmentTarget(ExplorerEntityKind.Group, new Topic(chat)));
                            }
                            break;
                    }
                }
                
                observer.OnNext(new Fetch
                {
                    Segment = new Segment("Bots", ExplorerEntityKind.Bot),
                    Targets = botTargets.Take(3).ToList()
                });
                
                observer.OnNext(new Fetch
                {
                    Segment = new Segment("Channels", ExplorerEntityKind.Channel),
                    Targets = channelTargets.Take(3).ToList()
                });
                
                observer.OnNext(new Fetch
                {
                    Segment = new Segment("Groups", ExplorerEntityKind.Group),
                    Targets = groupTargets.Take(3).ToList()
                });
                
                observer.OnNext(new Fetch
                {
                    Segment = new Segment("People", ExplorerEntityKind.People),
                    Targets = peopleTargets
                });
            });
        }

        public IObservable<Fetch> FetchByKind(ExplorerEntityKind kind)
        {
            return Observable.Create<Fetch>(async observer =>
            {
                var targets = new ReactiveList<SegmentTarget>();
                var chats = await GetAllChatsAsync();
                
                foreach (var chat in chats)
                {
                    switch (chat.Type)
                    {
                        case TD.ChatType.ChatTypePrivate ctp:
                            var user = await GetUserAsync(ctp.UserId);
                            if (user.Type is TD.UserType.UserTypeRegular)
                            {
                                targets.Add(new SegmentTarget(ExplorerEntityKind.People, new Topic(chat)));
                            }
                            else if (user.Type is TD.UserType.UserTypeBot)
                            {
                                targets.Add(new SegmentTarget(ExplorerEntityKind.Bot, new Topic(chat)));
                            }
                            break;
                        
                        case TD.ChatType.ChatTypeBasicGroup _:
                            targets.Add(new SegmentTarget(ExplorerEntityKind.Group, new Topic(chat)));
                            break;
                            
                        case TD.ChatType.ChatTypeSupergroup cts:
                            if (cts.IsChannel)
                            {
                                targets.Add(new SegmentTarget(ExplorerEntityKind.Channel, new Topic(chat)));
                            }
                            else
                            {
                                targets.Add(new SegmentTarget(ExplorerEntityKind.Group, new Topic(chat)));
                            }
                            break;
                    }
                }
                
                observer.OnNext(new Fetch
                {
                    Targets = targets.Where(t => t.Kind == kind).ToList()
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
        
            public IList<SegmentTarget> Targets { get; set; }
        }
    }
}