using System;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Microsoft.Extensions.Caching.Memory;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Shapes;
using TdLib;
using Tel.Egram.Persistance;
using Path = System.IO.Path;

namespace Tel.Egram.Graphics
{
    public class AvatarLoader : IAvatarLoader, IDisposable
    {
        private readonly IStorage _storage;
        private readonly IAvatarCache _cache;
        private readonly IFileLoader _fileLoader;
        private readonly IColorMapper _colorMapper;
        private readonly object _locker;

        public AvatarLoader(
            IStorage storage,
            IFileLoader fileLoader,
            IAvatarCache avatarCache,
            IColorMapper colorMapper)
        {
            _storage = storage;
            _fileLoader = fileLoader;
            _cache = avatarCache;
            _colorMapper = colorMapper;
            _locker = new object();
        }

        public IBitmap GetBitmap(TdApi.User user, AvatarSize size)
        {
            return GetBitmap(user.ProfilePhoto?.Small, user.Id, size);
        }

        public IBitmap GetBitmap(TdApi.Chat chat, AvatarSize size)
        {
            return GetBitmap(chat.Photo?.Small, chat.Id, size);
        }

        private IBitmap GetBitmap(TdApi.File file, long id, AvatarSize size)
        {
            var filename = GetFilename(file, id, size);

            if (_cache.TryGetValue(filename, out var bitmap))
            {
                return (IBitmap) bitmap;
            }

            return null;
        }

        public IObservable<IBitmap> LoadBitmap(TdApi.User user, AvatarSize size)
        {
            return LoadBitmap(user.ProfilePhoto?.Small, user.Id, size);
        }

        public IObservable<IBitmap> LoadBitmap(TdApi.Chat chat, AvatarSize size)
        {
            return LoadBitmap(chat.Photo?.Small, chat.Id, size);
        }

        private IObservable<IBitmap> LoadBitmap(TdApi.File file, long id, AvatarSize size)
        {
            var filename = GetFilename(file, id, size);
            
            if (file != null)
            {
                return _fileLoader.LoadFile(file, LoadPriority.Max)
                    .FirstAsync(f => f.Local != null && f.Local.IsDownloadingCompleted)
                    .SelectMany(f => PrepareAvatar(f.Local.Path, (int)size, filename).ToObservable())
                    .SelectMany(path => CreateBitmap(path).ToObservable())
                    .Do(bitmap =>
                    {
                        _cache.Set(filename, bitmap, new MemoryCacheEntryOptions
                        {
                            Size = 1
                        });
                    });
            }

            return Observable.Return<Bitmap>(null);
        }
        
        private Task<string> PrepareAvatar(string file, int size, string filename)
        {
            return Task.Run(() =>
            {
                var avatarFile = Path.Combine(_storage.AvatarDirectory, filename);

                // TODO: smarter locks based on filename
                lock (_locker)
                {
                    if (!File.Exists(avatarFile))
                    {
                        using (var source = Image.Load(file))
                        {
                            source.Mutate(ctx => ctx.Resize(size, size));
                            source.Save(avatarFile);
                        }
                    }
                }
                
                return avatarFile;
            });
        }

        private Task<Bitmap> CreateBitmap(string filePath)
        {
            return Task.Run(() => File.Exists(filePath) ? new Bitmap(filePath) : null);
        }

        private string GetFilename(TdApi.File file, long id, AvatarSize size)
        {
            var s = (int) size;
            var name = file == null ? _colorMapper[id] : id.ToString();
            return $"avatar_{s}x{s}_{name}.jpg";
        }
        
        public void Dispose()
        {
            _cache.Dispose();
        }
    }
}