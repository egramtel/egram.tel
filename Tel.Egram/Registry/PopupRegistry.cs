using Microsoft.Extensions.DependencyInjection;
using Tel.Egram.Components.Application;
using Tel.Egram.Components.Popup;

namespace Tel.Egram.Registry
{
    public static class PopupRegistry
    {
        public static void AddPopup(this IServiceCollection services)
        {
            services.AddScoped<IApplicationPopupController, ApplicationPopupController>();
            services.AddScoped<IPopupController>(p => p.GetService<IApplicationPopupController>());
        }
    }
}