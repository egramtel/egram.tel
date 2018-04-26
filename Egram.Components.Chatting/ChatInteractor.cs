using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Avalonia.Threading;
using Egram.Components.TDLib;
using ReactiveUI;

namespace Egram.Components.Chatting
{
    public class ChatInteractor : IDisposable
    {
        private readonly TD.Chat _chat;
        private readonly MessageLoader _messageLoader;
        private readonly NewMessageProvider _newMessageProvider;

        private readonly Subject<Action> _actionSubject;
        private readonly Subject<ChatEntity> _appendSubject;
        private readonly Subject<ChatEntity> _prependSubject;
        
        private readonly IDisposable _newMessageSubscription;
        private readonly IDisposable _actionsSubscription;
        
        public ChatInteractor(
            TD.Chat chat,
            MessageLoader messageLoader,
            NewMessageProvider newMessageProvider
            )
        {
            _chat = chat;
            
            _messageLoader = messageLoader;
            _newMessageProvider = newMessageProvider;
            
            _actionSubject = new Subject<Action>();
            _appendSubject = new Subject<ChatEntity>();
            _prependSubject = new Subject<ChatEntity>();
            
            _newMessageSubscription = _newMessageProvider
                .Appends(_chat)
                .Subscribe(_appendSubject);

            _actionsSubscription = _messageLoader
                .Actions()
                .Subscribe(_actionSubject);
        }

        public IObservable<Action> Actions()
        {
            return _actionSubject;
        }

        public IObservable<ChatEntity> Prepends()
        {
            return _prependSubject;
        }

        public IObservable<ChatEntity> Appends()
        {
            return _appendSubject;
        }

        public IObservable<Unit> LoadRecent()
        {
            return Observable.Create<Unit>(observer =>
            {
                return _messageLoader.LoadRecent(_chat)
                    .Buffer(TimeSpan.FromSeconds(10))
                    .Subscribe(messages =>
                    {
                        var entities = new List<ChatEntity>();

                        BaseMessage prevMessage = null;
                        foreach (var currentMessage in messages)
                        {
                            if (currentMessage != null)
                            {
                                if (prevMessage != null)
                                {
                                    if (prevMessage.DateTime.Year != currentMessage.DateTime.Year
                                        || prevMessage.DateTime.Month != currentMessage.DateTime.Month
                                        || prevMessage.DateTime.Day != currentMessage.DateTime.Day)
                                    {
                                        entities.Add(new DateBadge(currentMessage.DateTime));

                                        currentMessage.IsAddition = false;
                                        prevMessage.HasAddition = false;
                                    }
                                    else
                                    {
                                        currentMessage.HasAddition = currentMessage.MessageAuthor == prevMessage.MessageAuthor;
                                        prevMessage.IsAddition = currentMessage.HasAddition;
                                    }
                                }

                                entities.Add(currentMessage);
                                prevMessage = currentMessage;
                            }
                        }

                        foreach (var entity in entities)
                        {
                            _prependSubject.OnNext(entity);
                        }

                        observer.OnCompleted();
                    });
            });
        }

        public void Dispose()
        {
            _actionSubject.Dispose();
            _appendSubject.Dispose();
            _prependSubject.Dispose();
            
            _newMessageSubscription.Dispose();
            _actionsSubscription.Dispose();
        }
    }
}