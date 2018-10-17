using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Microsoft.Extensions.Caching.Memory;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
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

        private readonly ConcurrentDictionary<long, SolidColorBrush> _brushCache;
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
            
            _brushCache = new ConcurrentDictionary<long, SolidColorBrush>();
            _locker = new object();
        }

        public Avatar GetAvatar(TdApi.User user, AvatarSize size, bool forceFallback = false)
        {
            return new Avatar
            {
                Bitmap = forceFallback ? null : GetBitmap(user.ProfilePhoto?.Small, user.Id, size),
                BrushFactory = GetBrushFactory(user),
                Size = size,
                Label = GetLabel(user)
            };
        }

        public Avatar GetAvatar(TdApi.Chat chat, AvatarSize size, bool forceFallback = false)
        {
            return new Avatar
            {
                Bitmap = forceFallback ? null : GetBitmap(chat.Photo?.Small, chat.Id, size),
                BrushFactory = GetBrushFactory(chat),
                Size = size,
                Label = GetLabel(chat)
            };
        }

        public IObservable<Avatar> LoadAvatar(TdApi.User user, AvatarSize size)
        {
            return LoadBitmap(user.ProfilePhoto?.Small, user.Id, size)
                .Select(bitmap => new Avatar
                {
                    Bitmap = bitmap,
                    BrushFactory = GetBrushFactory(user),
                    Size = size,
                    Label = GetLabel(user)
                });
        }

        public IObservable<Avatar> LoadAvatar(TdApi.Chat chat, AvatarSize size)
        {
            return LoadBitmap(chat.Photo?.Small, chat.Id, size)
                .Select(bitmap => new Avatar
                {
                    Bitmap = bitmap,
                    BrushFactory = GetBrushFactory(chat),
                    Size = size,
                    Label = GetLabel(chat)
                });
        }

        private string GetLabel(TdApi.Chat chat)
        {
            var title = chat.Title;
            return string.IsNullOrWhiteSpace(title) ? null : title.Substring(0, 1).ToUpper();
        }

        private string GetLabel(TdApi.User user)
        {
            if (!string.IsNullOrWhiteSpace(user.FirstName) && !string.IsNullOrWhiteSpace(user.LastName))
            {
                return new string(new []{ user.FirstName[0], user.LastName[0] });
            }

            if (!string.IsNullOrWhiteSpace(user.FirstName))
            {
                return new string(user.FirstName[0], 1);
            }
            
            if (!string.IsNullOrWhiteSpace(user.LastName))
            {
                return new string(user.LastName[0], 1);
            }

            return null;
        }

        private Func<IBrush> GetBrushFactory(TdApi.User user)
        {
            return () =>
            {
                if (_brushCache.TryGetValue(user.Id, out var brush))
                {
                    return brush;
                }
                
                brush = new SolidColorBrush(Color.Parse("#" + _colorMapper[user.Id]));
                _brushCache.TryAdd(user.Id, brush);
                return brush;
            };
        }

        private Func<IBrush> GetBrushFactory(TdApi.Chat chat)
        {
            return () =>
            {
                if (_brushCache.TryGetValue(chat.Id, out var brush))
                {
                    return brush;
                }
                
                brush = new SolidColorBrush(Color.Parse("#" + _colorMapper[chat.Id]));
                _brushCache.TryAdd(chat.Id, brush);
                return brush;
            };
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