using Egram.Components.Chatting;
using Microsoft.Extensions.DependencyInjection;

namespace Egram.Registry
{
    public static class ChattingRegistry
    {
        public static void AddChatting(this IServiceCollection services)
        {
            services.AddScoped<MessageMapper>();
            services.AddScoped<MessageLoader>();
            services.AddScoped<NewMessageProvider>();
            services.AddScoped<MessageSender>();
            
            services.AddScoped<UserAuthorLoader>();
            services.AddScoped<ChannelAuthorLoader>();
        }
    }
}