using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Egram.Components.TDLib;

namespace Egram.Components.Chatting
{
    public class ChannelAuthorLoader : IDisposable
    {
        private readonly IAgent _agent;
        private readonly AvatarLoader _avatarLoader;
        private readonly ConcurrentDictionary<long, ChannelAuthor> _cache;
        
        public ChannelAuthorLoader(
            IAgent agent,
            AvatarLoader avatarLoader
            )
        {
            _agent = agent;
            _avatarLoader = avatarLoader;
            _cache = new ConcurrentDictionary<long, ChannelAuthor>();
        }
        
        public bool Retrieve(long chatId, out ChannelAuthor channelAuthor)
        {
            if (_cache.TryGetValue(chatId, out channelAuthor))
            {
                return true;
            }
            
            channelAuthor = new ChannelAuthor
            {
                ChatId = chatId
            };

            if (_cache.TryAdd(chatId, channelAuthor))
            {
                return false;
            }

            _cache.TryGetValue(chatId, out channelAuthor);
            
            return true;
        }

        public IObservable<Load> LoadAll(IList<long> chatIds)
        {
            return Observable.Create<Load>(async observer =>
            {
                var chats = new List<TD.Chat>(chatIds.Count);

                // load info
                foreach (var chatId in chatIds)
                {
                    if (_cache.TryGetValue(chatId, out var channelAuthor))
                    {
                        var chat = await _agent.ExecuteAsync(
                            new TD.GetChat
                            {
                                ChatId = chatId
                            });
                    
                        chats.Add(chat);
                    
                        observer.OnNext(new Load(
                            channelAuthor,
                            chat.Title
                        ));
                    }
                }
                
                // load avatars
                foreach (var chat in chats)
                {
                    if (_cache.TryGetValue(chat.Id, out var channelAuthor))
                    {
                        var bitmap = await _avatarLoader.LoadForChatAsync(chat);

                        observer.OnNext(new Load(
                            channelAuthor,
                            bitmap
                        ));
                    }
                }

                observer.OnCompleted();
            });
        }

        public void Dispose()
        {
            
        }
        
        public class Load
        {
            public readonly ChannelAuthor ChannelAuthor;
            public readonly IBitmap Avatar;
            public readonly string Name;

            public Load(ChannelAuthor channelAuthor, string name)
            {
                ChannelAuthor = channelAuthor;
                Name = name;
            }

            public Load(ChannelAuthor channelAuthor, IBitmap avatar)
            {
                ChannelAuthor = channelAuthor;
                Avatar = avatar;
            }
        }
    }
}