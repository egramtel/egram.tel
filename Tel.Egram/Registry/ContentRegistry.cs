using System;
using Microsoft.Extensions.DependencyInjection;
using Tel.Egram.Components.Content;
using Tel.Egram.Components.Content.Bots;
using Tel.Egram.Components.Content.Direct;
using Tel.Egram.Components.Content.Groups;
using Tel.Egram.Components.Content.Home;
using Tel.Egram.Utils;

namespace Tel.Egram.Registry
{
    public static class ContentRegistry
    {
        public static void AddContent(this IServiceCollection services)
        {
            services.AddScoped<IFactory<ContentKind, ContentContext>, ContentContextFactory>();
            
            services.AddScoped<HomeFeedInteractor>();
            services.AddTransient<HomeContentContext>();
            
            services.AddTransient<DirectContentContext>();
            
            services.AddTransient<GroupsContentContext>();
            
            services.AddTransient<BotsContentContext>();
        }
    }
}