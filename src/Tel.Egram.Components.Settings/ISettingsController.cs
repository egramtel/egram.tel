using System;
using Tel.Egram.Models.Settings;

namespace Tel.Egram.Components.Settings
{
    public interface ISettingsController : IDisposable
    {
        SettingsModel Model { get; }
    }
}