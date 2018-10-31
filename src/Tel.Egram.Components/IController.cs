using System;

namespace Tel.Egram.Components
{
    public interface IController<out T> : IController
    {
        T Model { get; }
    }

    public interface IController : IDisposable
    {
        T Activate<T>(ref IController<T> controller);
        T Activate<T, TArg>(TArg arg, ref IController<T> controller);
        T Deactivate<T>(ref IController<T> controller);
    }
}