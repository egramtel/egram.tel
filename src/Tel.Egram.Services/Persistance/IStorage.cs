namespace Tel.Egram.Services.Persistance
{
    public interface IStorage
    {
        string BaseDirectory { get; }

        string LogDirectory { get; }

        string TdLibDirectory { get; }
        
        string CacheDirectory { get; }
        
        string AvatarCacheDirectory { get; }
        
        string DatabaseFile { get; }
    }
}