using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using DynamicData;
using ReactiveUI;
using Splat;
using Tel.Egram.Model.Messenger.Explorer.Factories;
using Tel.Egram.Model.Messenger.Explorer.Messages;
using Tel.Egram.Services.Messaging.Chats;
using Tel.Egram.Services.Messaging.Messages;
using Tel.Egram.Services.Utils.Reactive;

namespace Tel.Egram.Model.Messenger.Explorer.Loaders
{
    public class NextMessageLoader
    {
        private readonly MessageLoaderConductor _conductor;
        
        private readonly IChatLoader _chatLoader;
        private readonly IMessageLoader _messageLoader;
        private readonly IMessageModelFactory _messageModelFactory;

        public NextMessageLoader(MessageLoaderConductor conductor)
            : this(
                Locator.Current.GetService<IChatLoader>(),
                Locator.Current.GetService<IMessageLoader>(),
                Locator.Current.GetService<IMessageModelFactory>())
        {
            _conductor = conductor;
        }
        
        public NextMessageLoader(
            IChatLoader chatLoader,
            IMessageLoader messageLoader,
            IMessageModelFactory messageModelFactory)
        {
            _chatLoader = chatLoader;
            _messageLoader = messageLoader;
            _messageModelFactory = messageModelFactory;
        }
        
        public IDisposable Bind(
            ExplorerModel model,
            Chat chat)
        {
            return model.WhenAnyValue(m => m.VisibleRange)
                .Throttle(TimeSpan.FromSeconds(1))
                .Select(r => r.LastIndex)
                .DistinctUntilChanged()
                .Where(index => model.SourceItems.Count != 0) // skip initial
                .Where(index => index + 4 > model.SourceItems.Count) // bottom is within 4 items
                .Where(index => !_conductor.IsBusy) // ignore if other load are already in progress
                .Synchronize(_conductor.Locker)
                .SelectSeq(r => StartLoading(model, chat))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Accept(list => HandleLoading(model, list));
        }
        
        private IObservable<IList<MessageModel>> StartLoading(
            ExplorerModel model,
            Chat chat)
        {
            //Console.WriteLine("Start next: {0}", Thread.CurrentThread.ManagedThreadId);
            _conductor.IsBusy = true;

            var fromMessage = model.SourceItems.Items
                .OfType<MessageModel>()
                .Last()
                .Message;
            
            return LoadNextMessages(chat, fromMessage)
                .ObserveOn(RxApp.TaskpoolScheduler)
                .SubscribeOn(RxApp.MainThreadScheduler)
                .Finally(() =>
                {
                    _conductor.IsBusy = false;
                });;
        }

        private void HandleLoading(
            ExplorerModel model,
            IList<MessageModel> messageModels)
        {
            //Console.WriteLine("End next");
            model.SourceItems.AddRange(messageModels);
        }
        
        public IObservable<IList<MessageModel>> LoadNextMessages(
            Chat chat,
            Message fromMessage)
        {
            return _chatLoader.LoadChat(chat.ChatData.Id)
                .SelectSeq(c => GetNextMessages(c, fromMessage)
                    .Select(_messageModelFactory.CreateMessage)
                    .ToList())
                .Select(list => list.Reverse().Skip(1).ToList());
        }

        private IObservable<Message> GetNextMessages(
            Chat chat,
            Message fromMessage)
        {
            var fromMessageId = fromMessage.MessageData.Id;
            return _messageLoader.LoadNextMessages(chat, fromMessageId, 32);
        }
    }
}