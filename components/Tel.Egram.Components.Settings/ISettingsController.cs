using System;
using Tel.Egram.Gui.Views.Settings;

namespace Tel.Egram.Components.Settings
{
    public interface ISettingsController : IDisposable
    {
        SettingsControlModel Model { get; }
    }
}