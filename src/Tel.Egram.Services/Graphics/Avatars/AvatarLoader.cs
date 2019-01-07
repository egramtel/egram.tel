using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reactive.Linq;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Microsoft.Extensions.Caching.Memory;
using TdLib;
using Tel.Egram.Services.Persistance;

namespace Tel.Egram.Services.Graphics.Avatars
{
    public class AvatarLoader : IAvatarLoader, IDisposable
    {
        private readonly IAvatarCache _cache;
        private readonly IFileLoader _fileLoader;
        private readonly IColorMapper _colorMapper;

        private readonly object _locker;

        public AvatarLoader(
            IFileLoader fileLoader,
            IAvatarCache avatarCache,
            IColorMapper colorMapper)
        {
            _fileLoader = fileLoader;
            _cache = avatarCache;
            _colorMapper = colorMapper;
            
            _locker = new object();
        }

        public Avatar GetAvatar(TdApi.User user, bool forceFallback = false)
        {
            return new Avatar
            {
                Bitmap = forceFallback ? null : GetBitmap(user.ProfilePhoto?.Small),
                Color = GetColor(user),
                Label = GetLabel(user)
            };
        }

        public Avatar GetAvatar(TdApi.Chat chat, bool forceFallback = false)
        {
            return new Avatar
            {
                Bitmap = forceFallback ? null : GetBitmap(chat.Photo?.Small),
                Color = GetColor(chat),
                Label = GetLabel(chat)
            };
        }

        public IObservable<Avatar> LoadAvatar(TdApi.User user)
        {
            return LoadBitmap(user.ProfilePhoto?.Small)
                .Select(bitmap => new Avatar
                {
                    Bitmap = bitmap,
                    Color = GetColor(user),
                    Label = GetLabel(user)
                });
        }

        public IObservable<Avatar> LoadAvatar(TdApi.Chat chat)
        {
            return LoadBitmap(chat.Photo?.Small)
                .Select(bitmap => new Avatar
                {
                    Bitmap = bitmap,
                    Color = GetColor(chat),
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

        private Color GetColor(TdApi.User user)
        {
            return Color.Parse("#" + _colorMapper[user.Id]);
        }

        private Color GetColor(TdApi.Chat chat)
        {
            return Color.Parse("#" + _colorMapper[chat.Id]);
        }

        private IBitmap GetBitmap(TdApi.File file)
        {
            if (file?.Local?.Path != null && _cache.TryGetValue(file.Local.Path, out var bitmap))
            {
                return (IBitmap) bitmap;
            }

            return null;
        }

        private IObservable<IBitmap> LoadBitmap(TdApi.File file)
        {   
            if (file != null)
            {
                return _fileLoader.LoadFile(file, LoadPriority.Max)
                    .FirstAsync(f => f.Local != null && f.Local.IsDownloadingCompleted)
                    .Select(f => GetBitmap(f.Local.Path));
            }

            return Observable.Return<Bitmap>(null);
        }

        private Bitmap GetBitmap(string filePath)
        {
            lock (_locker)
            {
                Bitmap bitmap = null;
            
                if (_cache.TryGetValue(filePath, out var item))
                {
                    bitmap = (Bitmap)item;
                }
                else if (File.Exists(filePath))
                {
                    bitmap = new Bitmap(filePath);
                    _cache.Set(filePath, bitmap, new MemoryCacheEntryOptions
                    {
                        Size = 1
                    });
                }
            
                return bitmap;
            }
        }
        
        public void Dispose()
        {
            _cache.Dispose();
        }
    }
}