using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;
using Splat;
using Tel.Egram.Components.Messenger.Explorer.Messages;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Utils;
using Tel.Egram.Utils.Reactive;

namespace Tel.Egram.Components.Messenger.Explorer
{
    public static class ExplorerLogic
    {
        public static IDisposable BindSource(
            this ExplorerModel model)
        {   
            return model.SourceItems.Connect()
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(model.Items)
                .Accept();
        }

        public static IDisposable InitMessageLoading(
            this ExplorerModel model,
            Chat chat)
        {
            return InitMessageLoading(
                model,
                chat,
                Locator.Current.GetService<IMessageManager>());
        }

        public static IDisposable InitMessageLoading(
            this ExplorerModel model,
            Chat chat,
            IMessageManager messageManager)
        {
            return messageManager.LoadInitMessages(chat)
                .ObserveOn(RxApp.TaskpoolScheduler)
                .SubscribeOn(RxApp.MainThreadScheduler)
                .Accept(messageModels =>
                {
                    model.SourceItems.InsertRange(messageModels, 0);
                });
        }
    }
}