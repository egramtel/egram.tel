using System;
using System.Reactive.Disposables;
using DynamicData.Binding;
using Splat;
using Tel.Egram.Components.Messenger.Explorer;
using Tel.Egram.Components.Messenger.Explorer.Messages;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Messaging.Messages;
using Tel.Egram.Utils.Reactive;

namespace Tel.Egram.Components.Messenger.Home
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