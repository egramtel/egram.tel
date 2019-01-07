using System;
using System.Reactive.Disposables;
using DynamicData.Binding;
using Splat;
using Tel.Egram.Model.Messenger.Explorer;
using Tel.Egram.Model.Messenger.Explorer.Factories;
using Tel.Egram.Model.Messenger.Explorer.Messages;
using Tel.Egram.Services.Messaging.Chats;
using Tel.Egram.Services.Messaging.Messages;
using Tel.Egram.Services.Utils.Reactive;

namespace Tel.Egram.Model.Messenger.Home
{
    public static class HomeLogic
    {
        public static IDisposable BindSearch(
            this HomeModel model)
        {
            return Disposable.Empty;
        }

        public static IDisposable BindPromoted(
            this HomeModel model)
        {
            return BindPromoted(
                model,
                Locator.Current.GetService<IChatLoader>(),
                Locator.Current.GetService<IMessageLoader>(),
                Locator.Current.GetService<IMessageModelFactory>());
        }

        public static IDisposable BindPromoted(
            this HomeModel model,
            IChatLoader chatLoader,
            IMessageLoader messageLoader,
            IMessageModelFactory messageModelFactory)
        {
            model.PromotedMessages = new ObservableCollectionExtended<MessageModel>();
            
            return chatLoader.LoadPromo()
                .SelectSeq(chat =>
                {
                    return messageLoader.LoadNextMessages(chat, 0, 10);
                })
                .Accept(message =>
                {
                    var messageModel = messageModelFactory.CreateMessage(message);
                    model.PromotedMessages.Add(messageModel);
                });
        }
    }
}