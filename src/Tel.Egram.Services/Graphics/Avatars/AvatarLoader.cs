using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Microsoft.Extensions.Caching.Memory;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using TdLib;
using Tel.Egram.Services.Persistance;
using Tel.Egram.Services.Utils.Platforms;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using Color = Avalonia.Media.Color;
using DrawingImage = System.Drawing.Image;
using DrawingBitmap = System.Drawing.Bitmap;
using DrawingGraphics = System.Drawing.Graphics;
using DrawingRectangle = System.Drawing.Rectangle;

namespace Tel.Egram.Services.Graphics.Avatars
{
    public class AvatarLoader : IAvatarLoader, IDisposable
    {
        private readonly IAvatarCache _cache;
        private readonly IPlatform _platform;
        private readonly IStorage _storage;
        private readonly IFileLoader _fileLoader;
        private readonly IColorMapper _colorMapper;

        private readonly object _locker;

        public AvatarLoader(
            IPlatform platform,
            IStorage storage,
            IFileLoader fileLoader,
            IAvatarCache avatarCache,
            IColorMapper colorMapper)
        {
            _platform = platform;
            _storage = storage;
            _fileLoader = fileLoader;
            _cache = avatarCache;
            _colorMapper = colorMapper;
            
            _locker = new object();
        }

        public Avatar GetAvatar(AvatarKind kind, AvatarSize size)
        {
            return new Avatar
            {
                Bitmap = null,
                Color = GetColor(kind),
                Label = GetLabel(kind)
            };
        }

        public Avatar GetAvatar(TdApi.User user, AvatarSize size)
        {
            int s = _platform.PixelDensity * (int) size;
            
            return new Avatar
            {
                Bitmap = GetBitmap(user.ProfilePhoto?.Small, s),
                Color = GetColor(user),
                Label = GetLabel(user)
            };
        }

        public Avatar GetAvatar(TdApi.Chat chat, AvatarSize size)
        {
            int s = _platform.PixelDensity * (int) size;
            
            return new Avatar
            {
                Bitmap = GetBitmap(chat.Photo?.Small, s),
                Color = GetColor(chat),
                Label = GetLabel(chat)
            };
        }

        public IObservable<Avatar> LoadAvatar(AvatarKind kind, AvatarSize size)
        {
            return Observable.Return(GetAvatar(kind, size));
        }

        public IObservable<Avatar> LoadAvatar(TdApi.User user, AvatarSize size)
        {
            int s = _platform.PixelDensity * (int) size;
            
            return LoadBitmap(user.ProfilePhoto?.Small, s)
                .Select(bitmap => new Avatar
                {
                    Bitmap = bitmap,
                    Color = GetColor(user),
                    Label = GetLabel(user)
                });
        }

        public IObservable<Avatar> LoadAvatar(TdApi.Chat chat, AvatarSize size)
        {
            int s = _platform.PixelDensity * (int) size;
            
            return LoadBitmap(chat.Photo?.Small, s)
                .Select(bitmap => new Avatar
                {
                    Bitmap = bitmap,
                    Color = GetColor(chat),
                    Label = GetLabel(chat)
                });
        }

        private string GetLabel(AvatarKind kind)
        {
            switch (kind)
            {
                case AvatarKind.Home:
                    return "@";
                default:
                    return "";
            }
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

        private Color GetColor(AvatarKind kind)
        {
            return Color.Parse("#" + _colorMapper[(int)kind]);
        }

        private Color GetColor(TdApi.User user)
        {
            return Color.Parse("#" + _colorMapper[user.Id]);
        }

        private Color GetColor(TdApi.Chat chat)
        {
            return Color.Parse("#" + _colorMapper[chat.Id]);
        }

        private IBitmap GetBitmap(TdApi.File file, int size)
        {
            if (file?.Local?.Path != null)
            {
                var resizedFilePath = GetResizedPath(file.Local.Path, size);
                if (_cache.TryGetValue(resizedFilePath, out var bitmap))
                {
                    return (IBitmap) bitmap;
                }
            }

            return null;
        }

        private IObservable<IBitmap> LoadBitmap(TdApi.File file, int size)
        {   
            if (file != null)
            {
                return _fileLoader.LoadFile(file, LoadPriority.Max)
                    .FirstAsync(f => f.Local != null && f.Local.IsDownloadingCompleted)
                    .SelectMany(f => GetBitmapAsync(f.Local.Path, size));
            }

            return Observable.Return<Bitmap>(null);
        }

        private Task<Bitmap> GetBitmapAsync(string filePath, int size)
        {
            var resizedFilePath = GetResizedPath(filePath, size);
            
            // return cached version
            if (_cache.TryGetValue(resizedFilePath, out var item))
            {
                var bitmap = (Bitmap) item;
                return Task.FromResult(bitmap);
            }
            
            // return resized version from disk
            if (File.Exists(resizedFilePath))
            {
                lock (_locker)
                {
                    if (File.Exists(resizedFilePath))
                    {
                        var bitmap = new Bitmap(resizedFilePath);
                        _cache.Set(resizedFilePath, bitmap, new MemoryCacheEntryOptions
                        {
                            Size = 1
                        });
                        return Task.FromResult(bitmap);
                    }
                }
            }
            
            // resize and return image
            if (File.Exists(filePath))
            {
                return Task.Run(() =>
                {
                    lock (_locker)
                    {
                        if (File.Exists(filePath) && !File.Exists(resizedFilePath))
                        {
                            if (_platform is WindowsPlatform)
                            {
                                ResizeWithSystemDrawing(filePath, resizedFilePath, size);
                            }
                            else
                            {
                                ResizeWithImageSharp(filePath, resizedFilePath, size);
                            }
                        }

                        if (File.Exists(resizedFilePath))
                        {
                            var bitmap = new Bitmap(resizedFilePath);
                            _cache.Set(resizedFilePath, bitmap, new MemoryCacheEntryOptions
                            {
                                Size = 1
                            });
                            return bitmap;
                        }

                        return null;
                    }
                });
            }

            return Task.FromResult<Bitmap>(null);
        }

        private string GetResizedPath(string localPath, int size)
        {   
            var originalName = Path.GetFileNameWithoutExtension(localPath);
            var originalExtension = Path.GetExtension(localPath);

            return Path.Combine(
                _storage.AvatarCacheDirectory,
                $"{originalName}_{size}{originalExtension}");
        }

        private void ResizeWithSystemDrawing(string filePath, string resizedFilePath, int size)
        {
            var image = DrawingImage.FromFile(filePath);
            
            var destRect = new DrawingRectangle(0, 0, size, size);
            var destImage = new DrawingBitmap(size, size);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = DrawingGraphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            destImage.Save(resizedFilePath);
        }

        private void ResizeWithImageSharp(string filePath, string resizedFilePath, int size)
        {
            using (SixLabors.ImageSharp.Image<Rgba32> image = SixLabors.ImageSharp.Image.Load(filePath))
            {
                image.Mutate(ctx=>ctx.Resize(size, size));
                image.Save(resizedFilePath);
            }
        }

        public void Dispose()
        {
            _cache.Dispose();
        }
    }
}