using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Egram.Components.Persistence;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Drawing;
using SixLabors.ImageSharp.Processing.Drawing.Brushes;
using SixLabors.ImageSharp.Processing.Overlays;
using SixLabors.ImageSharp.Processing.Transforms;
using SixLabors.Shapes;
using Path = System.IO.Path;

namespace Egram.Components.Graphics
{
    public class AvatarLoader
    {
        private readonly Storage _storage;
        private readonly FileLoader _fileLoader;
        private readonly ColorMaker _colorMaker;
        private readonly object _locker = new object();

        public AvatarLoader(
            Storage storage,
            FileLoader fileLoader,
            ColorMaker colorMaker
            )
        {
            _storage = storage;
            _fileLoader = fileLoader;
            _colorMaker = colorMaker;
        }

        public bool IsAvatarReady(TD.Chat chat, Size size)
        {
            var localFilePath = chat.Photo?.Small?.Local?.Path;
            if (!File.Exists(localFilePath))
            {
                return false;
            }
            
            var avatarFile = GetAvatarFilename(size, Path.GetFileNameWithoutExtension(localFilePath));
            
            return File.Exists(avatarFile);
        }

        public bool IsAvatarReady(TD.User user, Size size)
        {
            var localFilePath = user.ProfilePhoto?.Small?.Local?.Path;
            if (!File.Exists(localFilePath))
            {
                return false;
            }
            
            var avatarFile = GetAvatarFilename(size, Path.GetFileNameWithoutExtension(localFilePath));

            return File.Exists(avatarFile);
        }
        
        public async Task<IBitmap> LoadForUserAsync(TD.User user, Size size, bool forceFallback = false)
        {
            string avatarFile;
            if (forceFallback)
            {
                avatarFile = await CreateFallbackAvatar(user.Id, size);
            }
            else
            {
                var localFile = await _fileLoader.LoadFileAsync(user.ProfilePhoto?.Small);
            
                if (localFile?.Path != null)
                {
                    avatarFile = await CreateAvatar(localFile.Path, size);
                }
                else
                {
                    avatarFile = await CreateFallbackAvatar(user.Id, size);
                }
            }

            return await CreateBitmap(avatarFile);
        }

        public async Task<IBitmap> LoadForChatAsync(TD.Chat chat, Size size, bool forceFallback = false)
        {
            string avatarFile;
            if (forceFallback)
            {
                avatarFile = await CreateFallbackAvatar(chat.Id, size);
            }
            else
            {
                var localFile = await _fileLoader.LoadFileAsync(chat.Photo?.Small);
            
                if (localFile?.Path != null)
                {
                    avatarFile = await CreateAvatar(localFile.Path, size);
                }
                else
                {
                    avatarFile = await CreateFallbackAvatar(chat.Id, size);
                }
            }

            return await CreateBitmap(avatarFile);
        }

        private Task<string> CreateAvatar(string file, Size size)
        {
            return Task.Run(() =>
            {
                var s = (int)size;

                var avatarFile = GetAvatarFilename(size, Path.GetFileNameWithoutExtension(file));

                // TODO: smarter locks based on filename
                lock (_locker)
                {
                    if (!File.Exists(avatarFile))
                    {
                        using (var source = Image.Load(file))
                        using (var image = new Image<Rgba32>(s, s))
                        {
                            source.Mutate(ctx => ctx.Resize(s, s));
                        
                            var brush = new ImageBrush<Rgba32>(source);
                            var ellipse = new EllipsePolygon(s / 2.0f, s / 2.0f, s, s);
                            var options = new GraphicsOptions(true)
                            {
                                BlenderMode = PixelBlenderMode.Out
                            };
                        
                            image.Mutate(ctx =>
                                ctx.BackgroundColor(Rgba32.Transparent)
                                    .Fill(options, brush, ellipse));
                        
                            image.Save(avatarFile);
                        }
                    }
                }
                
                return avatarFile;
            });
        }

        private Task<string> CreateFallbackAvatar(long id, Size size)
        {
            return Task.Run(() =>
            {
                var color = _colorMaker.GetHexFromId(id);
                var s = (int)size;

                var avatarFile = GetAvatarFilename(size, color);

                // TODO: smarter locks based on filename
                lock (_locker)
                {
                    if (!File.Exists(avatarFile))
                    {
                        using (var image = new Image<Rgba32>(s, s))
                        {
                            var brush = new SolidBrush<Rgba32>(Rgba32.FromHex(color));
                            var ellipse = new EllipsePolygon(s / 2.0f, s / 2.0f, s, s);
                            var options = new GraphicsOptions(true)
                            {
                                BlenderMode = PixelBlenderMode.Out
                            };
                        
                            image.Mutate(ctx =>
                                ctx.BackgroundColor(Rgba32.Transparent)
                                    .Fill(options, brush, ellipse));
                        
                            image.Save(avatarFile);
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

        private string GetAvatarFilename(Size size, string name)
        {
            int s = (int) size;
            
            var avatarFile = Path.Combine(
                _storage.AvatarCacheDirectory,
                $"avatar_{s}x{s}_{name}.png");

            return avatarFile;
        }

        public enum Size
        {
            Explorer = 40,
            Chat = 80
        }
    }
}