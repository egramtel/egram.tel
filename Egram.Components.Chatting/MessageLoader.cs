using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Avalonia.Threading;
using Egram.Components.TDLib;

namespace Egram.Components.Chatting
{
    public class MessageLoader
    {
        private readonly IAgent _agent;
        private readonly MessageMapper _messageMapper;
        private readonly UserAuthorLoader _userAuthorLoader;
        private readonly ChannelAuthorLoader _channelAuthorLoader;

        private readonly Subject<Action> _actionSubject;

        public MessageLoader(
            IAgent agent,
            MessageMapper messageMapper,
            UserAuthorLoader userAuthorLoader,
            ChannelAuthorLoader channelAuthorLoader
            )
        {
            _agent = agent;
            _messageMapper = messageMapper;
            _userAuthorLoader = userAuthorLoader;
            _channelAuthorLoader = channelAuthorLoader;
            
            _actionSubject = new Subject<Action>();
        }

        private void ObserveUserAuthorLoad(UserAuthorLoader.Load load)
        {
            _actionSubject.OnNext(() =>
            {
                var userAuthor = load.UserAuthor;
            
                if (load.Avatar != null)
                {
                    userAuthor.Avatar = load.Avatar;
                }

                if (load.Name != null)
                {
                    userAuthor.Name = load.Name;
                }
            });
        }

        private void ObserveChannelAuthorLoad(ChannelAuthorLoader.Load load)
        {
            _actionSubject.OnNext(() =>
            {
                var channelAuthor = load.ChannelAuthor;
            
                if (load.Avatar != null)
                {
                    channelAuthor.Avatar = load.Avatar;
                }

                if (load.Name != null)
                {
                    channelAuthor.Name = load.Name;
                }
            });
        }

        public IObservable<Action> Actions()
        {
            return _actionSubject;
        }

        public IObservable<BaseMessage> LoadRecent(TD.Chat chat)
        {
            return Load(chat, 0, 50);
        }

        public IObservable<BaseMessage> Load(TD.Chat chat, long fromMessageId, int limit)
        {
            return Observable.Create<BaseMessage>(async observer =>
            {
                var chatsToLoad = new HashSet<long>();
                var usersToLoad = new HashSet<int>();

                while (true)
                {
                    var messages = await _agent.ExecuteAsync(new TD.GetChatHistory
                    {
                        ChatId = chat.Id,
                        FromMessageId = fromMessageId,
                        Offset = 0,
                        Limit = limit,
                        OnlyLocal = false
                    });

                    foreach (var m in messages.Messages_)
                    {
                        MessageAuthor messageAuthor;
                        if (chat.Type is TD.ChatType.ChatTypeSupergroup cts && cts.IsChannel)
                        {
                            if (!_channelAuthorLoader.Retrieve(chat.Id, out var channelAuthor))
                            {
                                chatsToLoad.Add(chat.Id);
                            }

                            messageAuthor = channelAuthor;
                        }
                        else
                        {
                            if (!_userAuthorLoader.Retrieve(m.SenderUserId, out var userAuthor))
                            {
                                usersToLoad.Add(m.SenderUserId);
                            }

                            messageAuthor = userAuthor;
                        }
                    
                        var message = _messageMapper.Map(m, messageAuthor);
                        observer.OnNext(message);
                    }
                    
                    limit -= messages.Messages_.Length;

                    if (messages.Messages_.Length == 0 || limit <= 0)
                    {
                        break;
                    }

                    fromMessageId = messages.Messages_[messages.Messages_.Length - 1].Id;
                }
                
                observer.OnCompleted();
    
                _userAuthorLoader.LoadAll(usersToLoad.ToList())
                    .Subscribe(ObserveUserAuthorLoad);
                
                _channelAuthorLoader.LoadAll(chatsToLoad.ToList())
                    .Subscribe(ObserveChannelAuthorLoad);
            });
        }
    }
}