using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Avalonia.Threading;
using Egram.Components.Navigation;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace Egram.Components.Chatting
{
    public class ChatContext : ReactiveObject, IDisposable
    {
        private readonly Topic _topic;
        private readonly ChatInteractor _chatInteractor;
        private readonly MessageSender _messageSender;
        
        private readonly IDisposable _actionSubscription;
        private readonly IDisposable _appendSubscription;
        private readonly IDisposable _prependSubscription;
        
        public ChatContext(
            Topic topic,
            ChatInteractor chatInteractor,
            MessageSender messageSender
            )
        {
            _topic = topic;
            _chatInteractor = chatInteractor;
            _messageSender = messageSender;
            
            _prependSubscription = _chatInteractor
                .Prepends()
                .Buffer(TimeSpan.FromSeconds(1), 16)
                .Where(list => list.Count > 0)
                .ObserveOn(AvaloniaScheduler.Instance)
                .Subscribe(ObservePrepends);

            _appendSubscription = _chatInteractor
                .Appends()
                .Buffer(TimeSpan.FromSeconds(1), 16)
                .Where(list => list.Count > 0)
                .ObserveOn(AvaloniaScheduler.Instance)
                .Subscribe(ObserveAppends);

            _actionSubscription = _chatInteractor
                .Actions()
                .ObserveOn(AvaloniaScheduler.Instance)
                .Subscribe(ObserveActions);

            SendCommand = ReactiveCommand.Create(() =>
            {
                var messageText = new TD.InputMessageContent.InputMessageText
                {
                    Text = new TD.FormattedText
                    {
                        Text = _editorText
                    },
                    ClearDraft = true
                };
                
                _messageSender
                    .SendText(topic.Chat, messageText)
                    .Subscribe(ObserveSendResult, ObserveSendError, ObserveSendComplete);
            });
            
            Entities = new ReactiveList<ChatEntity>();
            
            _chatInteractor
                .LoadRecent()
                .ObserveOn(AvaloniaScheduler.Instance)
                .Subscribe(ObserveLoadRecent);
        }

        private void ObservePrepends(IList<ChatEntity> entities)
        {   
            Entities.InsertRange(0, entities.Reverse());
        }

        private void ObserveAppends(IList<ChatEntity> entities)
        {
            Entities.AddRange(entities);
        }

        private void ObserveActions(Action action)
        {
            action();
        }

        private void ObserveLoadRecent(Unit _)
        {
            //Messages = new ReactiveList<ChatEntity>(entities);
        }

        private void ObserveSendResult(TD.Message message)
        {
            //Messages.Add(message);
        }

        private void ObserveSendError(Exception exception)
        {
            
        }

        private void ObserveSendComplete()
        {
            EditorText = "";
        }
        
        public ReactiveCommand<Unit, Unit> SendCommand { get; }

        private string _editorText;
        public string EditorText
        {
            get => _editorText;
            set => this.RaiseAndSetIfChanged(ref _editorText, value);
        }

        private IReactiveList<ChatEntity> _entities;
        public IReactiveList<ChatEntity> Entities
        {
            get => _entities;
            set => this.RaiseAndSetIfChanged(ref _entities, value);
        }

        public void Dispose()
        {
            _chatInteractor.Dispose();
            
            _actionSubscription.Dispose();
            _prependSubscription.Dispose();
            _appendSubscription.Dispose();
        }
    }

    public class ChatContextFactory
    {
        private readonly IServiceProvider _provider;

        public ChatContextFactory(IServiceProvider provider)
        {
            _provider = provider;
        }
        
        public ChatContext FromTopic(Topic topic)
        {
            return new ChatContext(
                topic,
                new ChatInteractor(
                    topic.Chat,
                    _provider.GetService<MessageLoader>(),
                    _provider.GetService<NewMessageProvider>()),
                _provider.GetService<MessageSender>());
        }
    }
}