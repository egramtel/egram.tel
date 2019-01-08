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
using Tel.Egram.Model.Messenger.Explorer.Items;
using Tel.Egram.Model.Messenger.Explorer.Messages;
using Tel.Egram.Services.Messaging.Chats;
using Tel.Egram.Services.Messaging.Messages;
using Tel.Egram.Services.Utils.Reactive;

namespace Tel.Egram.Model.Messenger.Explorer.Loaders
{
    public class PrevMessageLoader
    {
        private readonly MessageLoaderConductor _conductor;
        
        private readonly IChatLoader _chatLoader;
        private readonly IMessageLoader _messageLoader;
        private readonly IMessageModelFactory _messageModelFactory;

        public PrevMessageLoader(MessageLoaderConductor conductor)
            : this(
                Locator.Current.GetService<IChatLoader>(),
                Locator.Current.GetService<IMessageLoader>(),
                Locator.Current.GetService<IMessageModelFactory>())
        {
            _conductor = conductor;
        }
        
        public PrevMessageLoader(
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
                .Select(r => r.Index)
                .DistinctUntilChanged()
                .Where(index => model.SourceItems.Count != 0) // skip initial
                .Where(index => index - 4 < 0) // top is within 4 items
                .Where(index => !_conductor.IsBusy) // ignore if other load are already in progress
                .Synchronize(_conductor.Locker)
                .SelectSeq(r => StartLoading(model, chat))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Accept(list => HandleLoading(model, chat, list));
        }
        
        private IObservable<IList<MessageModel>> StartLoading(
            ExplorerModel model,
            Chat chat)
        {
            //Console.WriteLine("Start prev: {0}", Thread.CurrentThread.ManagedThreadId);
            _conductor.IsBusy = true;

            var fromMessage = model.SourceItems.Items
                .OfType<MessageModel>()
                .First()
                .Message;
            
            return LoadPrevMessages(chat, fromMessage)
                .ObserveOn(RxApp.TaskpoolScheduler)
                .SubscribeOn(RxApp.MainThreadScheduler)
                .Finally(() =>
                {
                    _conductor.IsBusy = false;
                });;
        }

        private void HandleLoading(
            ExplorerModel model,
            Chat chat,
            IList<MessageModel> messageModels)
        {
            //Console.WriteLine("End prev");

            ItemModel targetItem = null;

            // find item which is currently visible to scroll to it later
            if (model.SourceItems.Count > 0)
            {
                targetItem = model.SourceItems.Items
                    .Skip(model.VisibleRange.Index)
                    .OfType<MessageModel>()
                    .FirstOrDefault();
            }

            // or take last added item
            if (targetItem == null && messageModels.Count > 0)
            {
                targetItem = messageModels[messageModels.Count - 1];
            }

            model.TargetItem = targetItem;
            model.SourceItems.InsertRange(messageModels, 0);
        }
        
        public IObservable<IList<MessageModel>> LoadPrevMessages(
            Chat chat, 
            Message fromMessage)
        {
            return _chatLoader.LoadChat(chat.ChatData.Id)
                .SelectSeq(c => GetPrevMessages(c, fromMessage)
                    .Select(_messageModelFactory.CreateMessage)
                    .ToList())
                .Select(list => list.Reverse().ToList());
        }

        private IObservable<Message> GetPrevMessages(
            Chat chat,
            Message fromMessage)
        {
            var fromMessageId = fromMessage.MessageData.Id;
            return _messageLoader.LoadPrevMessages(chat, fromMessageId, 32);
        }
    }
}