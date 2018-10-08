using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Media;
using ReactiveUI;
using TdLib;
using Tel.Egram.Graphics;
using Tel.Egram.Messaging.Chats;

namespace Tel.Egram.Components.Messenger
{
    public class ChatMessengerContext : MessengerContext
    {
        public ChatMessengerContext(
            IAvatarLoader avatarLoader,
            IColorMapper colorMapper,
            IChatService chatService,
            Chat chat
            )
        {
            LoadChatInfo(chat, avatarLoader, colorMapper)
                .DisposeWith(_contextDisposable);
            
            LoadMessageEditor(chat);
        }

        private IDisposable LoadChatInfo(Chat chat, IAvatarLoader avatarLoader, IColorMapper colorMapper)
        {
            var chatData = chat.ChatData;

            ChatInfo = new ChatInfoModel
            {
                Title = chatData.Title,
                Label = "test"
            };

            return avatarLoader.LoadAvatar(chatData, AvatarSize.Big)
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(avatar =>
                {
                    ChatInfo.Avatar = avatar;
                });
        }

        private void LoadMessageEditor(Chat chat)
        {
            if (chat.ChatData.Type is TdApi.ChatType.ChatTypeSupergroup supergroup)
            {
                //IsMessageEditorVisible = supergroup.IsChannel;
            }
        }
    }
}