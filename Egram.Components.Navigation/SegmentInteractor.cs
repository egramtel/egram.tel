using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Egram.Components.Graphics;
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

        public IObservable<Result> FetchAggregated()
        {
            return FetchAll().SelectMany(result =>
            {
                var results = new List<Result>();

                switch (result)
                {
                    case Fetch f:
                        results.Add(new Fetch(
                            new Segment
                            {
                                Name = "Bots",
                                Kind = ExplorerEntityKind.Header | ExplorerEntityKind.Bot
                            },
                            f.Conversations.Where(c => c.Kind == ExplorerEntityKind.Bot)
                                .Take(3)
                                .ToList()));

                        results.Add(new Fetch(
                            new Segment
                            {
                                Name = "Channels",
                                Kind = ExplorerEntityKind.Header | ExplorerEntityKind.Channel
                            },
                            f.Conversations.Where(c => c.Kind == ExplorerEntityKind.Channel)
                                .Take(3)
                                .ToList()));

                        results.Add(new Fetch(
                            new Segment
                            {
                                Name = "Groups",
                                Kind = ExplorerEntityKind.Header | ExplorerEntityKind.Group
                            },
                            f.Conversations.Where(c => c.Kind == ExplorerEntityKind.Group)
                                .Take(3)
                                .ToList()));

                        results.Add(new Fetch(
                            new Segment
                            {
                                Name = "People",
                                Kind = ExplorerEntityKind.Header | ExplorerEntityKind.People
                            },
                            f.Conversations.Where(c => c.Kind == ExplorerEntityKind.People)
                                .ToList()));
                        break;
                    
                    default:
                        results.Add(result);
                        break;
                }
                
                return results;
            });
        }

        public IObservable<Result> FetchByKind(ExplorerEntityKind kind)
        {
            return FetchAll().Select(result =>
            {
                switch (result)
                {
                    case Fetch f:
                        return new Fetch(f.Conversations.Where(t => t.Kind == kind).ToList());
                    default:
                        return result;
                }
            });
        }

        public IObservable<Result> FetchAll()
        {
            return Observable.Create<Result>(async observer =>
            {
                var conversations = new List<Conversation>();
                var chats = await GetAllChatsAsync();
                var conversationsToLoad = new List<Conversation>();
                
                foreach (var chat in chats)
                {
                    Conversation conversation;
                    
                    switch (chat.Type)
                    {
                        case TD.ChatType.ChatTypePrivate ctp:
                            var user = await GetUserAsync(ctp.UserId);
                            if (!_conversationLoader.Retrieve(chat, user, out conversation))
                            {
                                conversationsToLoad.Add(conversation);
                            }
                            break;
                        
                        default:
                            if (!_conversationLoader.Retrieve(chat, out conversation))
                            {
                                conversationsToLoad.Add(conversation);
                            }
                            break;
                    }

                    if (conversation != null)
                    {
                        conversations.Add(conversation);
                    }
                }

                observer.OnNext(new Fetch(conversations));

                var nextToLoad = new List<Conversation>();
                
                // fast loading from disk
                foreach (var conversation in conversationsToLoad)
                {
                    if (_avatarLoader.IsAvatarReady(conversation.Chat, AvatarLoader.Size.Explorer))
                    {
                        var bitmap = await _avatarLoader.LoadForChatAsync(conversation.Chat, AvatarLoader.Size.Explorer);
                        observer.OnNext(new Update(conversation, bitmap));
                    }
                    else
                    {
                        nextToLoad.Add(conversation);
                    }
                }
                
                // slow loading (networking, processing, etc.)
                foreach (var conversation in nextToLoad)
                {
                    var bitmap = await _avatarLoader.LoadForChatAsync(conversation.Chat, AvatarLoader.Size.Explorer);
                    observer.OnNext(new Update(conversation, bitmap));
                }
                
                observer.OnCompleted();
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
        
        public class Update : Result
        {
            public readonly Conversation Conversation;
            public readonly IBitmap Avatar;

            public Update(Conversation conversation, IBitmap avatar)
            {
                Conversation = conversation;
                Avatar = avatar;
            }
        }
        
        public class Fetch : Result
        {
            public readonly Segment Segment;
            public readonly IList<Conversation> Conversations;

            public Fetch(Segment segment, IList<Conversation> conversations)
            {
                Segment = segment;
                Conversations = conversations;
            }

            public Fetch(IList<Conversation> conversations)
            {
                Conversations = conversations;
            }
        }
        
        public class Result
        {
            
        }
    }
}