using Microsoft.Extensions.DependencyInjection;
using Tel.Egram.Components.Settings;
using Tel.Egram.Components.Settings.Connection;

namespace Tel.Egram.Registry
{
    public static class SettingsRegistry
    {
        public static void AddSettings(this IServiceCollection services)
        {
            services.AddTransient<SettingsModel>();
            services.AddTransient<ProxyPopupModel>();
        }
    }
}