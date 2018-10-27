using System;
using Tel.Egram.Gui.Views.Application;

namespace Tel.Egram.Components.Application
{
    public interface IApplicationController : IDisposable
    {
        MainWindowModel Model { get; }
    }
}