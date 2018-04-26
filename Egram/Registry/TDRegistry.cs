using System.IO;
using Egram.Components.Persistence;
using Egram.Components.TDLib;
using Microsoft.Extensions.DependencyInjection;
using TD;

namespace Egram.Registry
{
    public static class TDRegistry
    {
        public static void AddTDLib(this IServiceCollection services)
        {
            services.AddSingleton(p =>
            {
                Client.Log.SetVerbosityLevel(5);
                Client.Log.SetFilePath(Path.Combine(p.GetService<Storage>().LogDirectory, "tdlib.log"));
                return new Client();
            });

            services.AddSingleton<Hub>();
            services.AddSingleton<Dialer>();
            services.AddSingleton<IAgent, Agent>();
        }
    }
}