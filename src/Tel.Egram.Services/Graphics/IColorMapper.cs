namespace Tel.Egram.Services.Graphics
{
    public interface IColorMapper
    {
        string this[long id]
        {
            get;
        }
    }
}