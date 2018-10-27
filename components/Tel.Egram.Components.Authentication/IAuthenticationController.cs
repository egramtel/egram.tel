using System;
using Tel.Egram.Gui.Views.Authentication;

namespace Tel.Egram.Components.Authentication
{
    public interface IAuthenticationController : IDisposable
    {
        AuthenticationPageModel Model { get; }
    }
}