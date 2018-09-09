using System;
using Microsoft.Extensions.DependencyInjection;
using Tel.Egram.Components.Navigation;
using Tel.Egram.Components.Workspace;
using Tel.Egram.Graphics;
using Tel.Egram.TdLib;

namespace Tel.Egram.Registry
{
    public static class WorkspaceRegistry
    {
        public static void AddWorkspace(this IServiceCollection services)
        {
            services.AddTransient<WorkspaceContext>();
        }
    }
}