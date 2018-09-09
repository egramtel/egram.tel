using System;
using Microsoft.Extensions.DependencyInjection;
using Tel.Egram.Components.Messenger;

namespace Tel.Egram.Registry
{
    public static class MessengerRegistry
    {
        public static void AddMessenger(this IServiceCollection services)
        {
            services.AddScoped<AggregateMessengerContext>();
            services.AddScoped<ChatMessengerContext>();
        }
    }
}