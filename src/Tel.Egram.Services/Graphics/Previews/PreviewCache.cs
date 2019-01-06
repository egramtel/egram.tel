using Microsoft.Extensions.Caching.Memory;

namespace Tel.Egram.Services.Graphics.Previews
{
    public class PreviewCache : IPreviewCache
    {
        private readonly IMemoryCache _cache;

        public PreviewCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        public bool TryGetValue(object key, out object value)
        {
            return _cache.TryGetValue(key, out value);
        }

        public ICacheEntry CreateEntry(object key)
        {
            return _cache.CreateEntry(key);
        }

        public void Remove(object key)
        {
            _cache.Remove(key);
        }

        public void Dispose()
        {
            _cache.Dispose();
        }
    }
}