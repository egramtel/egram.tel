using System;
using System.Collections.Generic;
using System.Linq;
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
    public class InitMessageLoader
    {
        private readonly MessageLoaderConductor _conductor;
        
        private readonly IChatLoader _chatLoader;
        private readonly IMessageLoader _messageLoader;
        private readonly IMessageModelFactory _messageModelFactory;

        public InitMessageLoader(
            MessageLoaderConductor conductor)
            : this(
                Locator.Current.GetService<IChatLoader>(),
                Locator.Current.GetService<IMessageLoader>(),
                Locator.Current.GetService<IMessageModelFactory>())
        {
            _conductor = conductor;
        }
        
        public InitMessageLoader(
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
                .Where(r => model.SourceItems.Count == 0) // only initial
                .Where(r => !_conductor.IsBusy) // ignore if other load are already in progress
                .Synchronize(_conductor.Locker)
                .SelectSeq(r => StartLoading(chat))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Accept(list => HandleLoading(model, chat, list));
        }

        private IObservable<IList<MessageModel>> StartLoading(
            Chat chat)
        {
            //Console.WriteLine("Start init: {0}", Thread.CurrentThread.ManagedThreadId);
            _conductor.IsBusy = true;
            
            return LoadInitMessages(chat)
                .ObserveOn(RxApp.TaskpoolScheduler)
                .SubscribeOn(RxApp.MainThreadScheduler)
                .Finally(() =>
                {
                    _conductor.IsBusy = false;
                });
        }

        private void HandleLoading(
            ExplorerModel model,
            Chat chat,
            IList<MessageModel> messageModels)
        {
            //Console.WriteLine("End init");
            
            // find last read message to scroll to it later
            var targetItem = messageModels
                .FirstOrDefault(m => m.Message.MessageData.Id == chat.ChatData.LastReadInboxMessageId);

            if (targetItem == null && messageModels.Count > 0)
            {
                targetItem = messageModels[messageModels.Count / 2];
            }

            model.SourceItems.AddRange(messageModels);
            model.TargetItem = targetItem;
        }
        
        private IObservable<IList<MessageModel>> LoadInitMessages(
            Chat chat)
        {
            return _chatLoader.LoadChat(chat.ChatData.Id)
                .SelectSeq(c => GetInitMessages(c)
                    .Select(_messageModelFactory.CreateMessage)
                    .ToList())
                .Select(list => list.Reverse().ToList());
        }

        private IObservable<Message> GetInitMessages(
            Chat chat)
        {
            var fromMessageId = chat.ChatData.LastReadInboxMessageId;
            return _messageLoader.LoadInitMessages(chat, fromMessageId, 32);
        }
    }
}