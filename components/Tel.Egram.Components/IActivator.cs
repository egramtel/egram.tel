namespace Tel.Egram.Components
{
    public interface IActivator<T>
        where T : class, new()
    {
        T Activate(ref IController<T> controller);
        T Deactivate(ref IController<T> controller);
    }

    public interface IActivator<TArg, T>
        where T : class, new()
    {
        T Activate(TArg arg, ref IController<T> controller);
        T Deactivate(ref IController<T> controller);
    }
}