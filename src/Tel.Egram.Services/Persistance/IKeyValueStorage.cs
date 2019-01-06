using System.Collections.Generic;

namespace Tel.Egram.Services.Persistance
{
    public interface IKeyValueStorage
    {
        void Set<T>(string key, T value);
        
        T Get<T>(string key);

        IList<KeyValuePair<string, T>> GetAll<T>();
        
        bool TryGet<T>(string key, out T value);

        void Delete(string key);
    }
}