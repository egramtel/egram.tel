using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using TdLib;
using Tel.Egram.Feeds;
using Tel.Egram.Messages;

namespace Tel.Egram.Components.Messenger.Aggregate
{
    public class AggregateMessageInteractor
    {
        private readonly IMessageLoader _messageLoader;
        private AggregateLoading _aggregateLoading;

        public AggregateMessageInteractor(
            IMessageLoader messageLoader
            )
        {
            _messageLoader = messageLoader;
        }

        public IDisposable LoadInitialMessages(AggregateMessengerContext context,  AggregateFeed feed)
        {
            _aggregateLoading = new AggregateLoading();
            
            return _messageLoader.LoadMessages(feed, _aggregateLoading)
                .Aggregate(new List<Message>(), (list, message) =>
                {
                    list.Add(message);
                    return list;
                })
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(AvaloniaScheduler.Instance)
                .Subscribe(messages =>
                {
                    context.OnAppendMessages(messages);
                });
        }
    }
}