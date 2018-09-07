using System;
using Microsoft.Extensions.DependencyInjection;
using Tel.Egram.Components.Messenger;
using Tel.Egram.Components.Messenger.Aggregate;
using Tel.Egram.Components.Messenger.Chats;
using Tel.Egram.Feeds;
using Tel.Egram.Utils;

namespace Tel.Egram.Registry
{
    public static class MessengerRegistry
    {
        public static void AddMessenger(this IServiceCollection services)
        {
            services.AddScoped<AggregateMessengerContext>();
            services.AddScoped<ChatMessengerContext>();
            services.AddScoped<AggregateMessageInteractor>();
            services.AddScoped<MessageAvatarInteractor>();
        }
    }
}