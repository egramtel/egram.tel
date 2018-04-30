using System;
using System.Collections.Concurrent;
using Egram.Components.Chatting;
using Egram.Components.TDLib;

namespace Egram.Components.Navigation
{
    public class ConversationLoader : IDisposable
    {
        private readonly IAgent _agent;
        private readonly AvatarLoader _avatarLoader;
        private readonly ConcurrentDictionary<long, Conversation> _cache;

        public ConversationLoader(
            IAgent agent,
            AvatarLoader avatarLoader
            )
        {
            _agent = agent;
            _avatarLoader = avatarLoader;
            _cache = new ConcurrentDictionary<long, Conversation>();
        }
        
        public bool Retrieve(long chatId, out Conversation conversation)
        {
            if (_cache.TryGetValue(chatId, out conversation))
            {
                return true;
            }
            
            conversation = new Conversation
            {
                
            };

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