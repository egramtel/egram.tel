using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Egram.Components.Chatting;
using Egram.Components.TDLib;

namespace Egram.Components.Navigation
{
    public class ConversationLoader : IDisposable
    {
        private readonly AvatarLoader _avatarLoader;
        private readonly ConcurrentDictionary<long, Conversation> _cache;

        public ConversationLoader(
            AvatarLoader avatarLoader
            )
        {
            _avatarLoader = avatarLoader;
            _cache = new ConcurrentDictionary<long, Conversation>();
        }

        public bool Retrieve(TD.Chat chat, TD.User user, out Conversation conversation)
        {
            var result = Retrieve(chat.Id, out conversation);
            
            switch (user.Type)
            {
                case TD.UserType.UserTypeRegular _:
                    conversation.Kind = ExplorerEntityKind.People;
                    break;
                
                case TD.UserType.UserTypeBot _:
                    conversation.Kind = ExplorerEntityKind.Bot;
                    break;
            }
            
            conversation.Title = chat.Title;
            conversation.Chat = chat;
            
            return result;
        }

        public bool Retrieve(TD.Chat chat, out Conversation conversation)
        {
            var result = Retrieve(chat.Id, out conversation);
            
            switch (chat.Type)
            {           
                case TD.ChatType.ChatTypeBasicGroup _:
                    conversation.Kind = ExplorerEntityKind.Group;
                    break;
                            
                case TD.ChatType.ChatTypeSupergroup cts:
                    conversation.Kind = cts.IsChannel ? ExplorerEntityKind.Channel : ExplorerEntityKind.Group;
                    break;
            }
            
            conversation.Title = chat.Title;
            conversation.Chat = chat;
            
            return result;
        }

        public Task<IBitmap> LoadAvatar(Conversation conversation)
        {
            return _avatarLoader.LoadForChatAsync(conversation.Chat);
        }
        
        private bool Retrieve(long chatId, out Conversation conversation)
        {
            if (_cache.TryGetValue(chatId, out conversation))
            {
                return true;
            }

            conversation = new Conversation();

            if (_cache.TryAdd(chatId, conversation))
            {
                return false;
            }

            _cache.TryGetValue(chatId, out conversation);
            
            return true;
        }

        public void Dispose()
        {
            
        }
    }
}