using System;

namespace Tel.Egram.Components
{
    public interface IController<out T> : IDisposable
    {
        T Model { get; }
    }
}