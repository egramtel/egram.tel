using Microsoft.Extensions.DependencyInjection;
using Tel.Egram.Components.Catalog;

namespace Tel.Egram.Registry
{
    public static class CatalogRegistry
    {
        public static void AddCatalog(this IServiceCollection services)
        {
            services.AddTransient<IEntryLoader, EntryLoader>();
            services.AddTransient<CatalogContext>();
        }
    }
}