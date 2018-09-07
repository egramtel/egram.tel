namespace Tel.Egram.Utils
{
    public interface IFactory<out TResult>
    {
        TResult Create();
    }

    public interface IFactory<in TParam1, out TResult>
    {
        TResult Create(TParam1 param1);
    }
}