using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Egram.Components.Persistence;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Overlays;
using SixLabors.ImageSharp.Processing.Transforms;

namespace Egram.Components.Graphics
{
    public class AvatarLoader
    {
        private readonly Storage _storage;
        private readonly FileLoader _fileLoader;

        public AvatarLoader(
            Storage storage,
            FileLoader fileLoader
            )
        {
            _storage = storage;
            _fileLoader = fileLoader;
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
        
        public async Task<IBitmap> LoadForUserAsync(TD.User user, Size size)
        {   
            var localFile = await _fileLoader.LoadFileAsync(user.ProfilePhoto?.Small);

            string avatarFile;
            if (localFile?.Path != null)
            {
                avatarFile = await CreateAvatar(localFile.Path, size);
            }
            else
            {
                avatarFile = await CreateFallbackAvatar(user.Id, size);
            }

            return await CreateBitmap(avatarFile);
        }

        public async Task<IBitmap> LoadForChatAsync(TD.Chat chat, Size size)
        {   
            var localFile = await _fileLoader.LoadFileAsync(chat.Photo?.Small);

            string avatarFile;
            if (localFile?.Path != null)
            {
                avatarFile = await CreateAvatar(localFile.Path, size);
            }
            else
            {
                avatarFile = await CreateFallbackAvatar(chat.Id, size);
            }

            return await CreateBitmap(avatarFile);
        }

        private Task<string> CreateAvatar(string file, Size size)
        {
            return Task.Run(() =>
            {
                var s = (int)size;

                var avatarFile = GetAvatarFilename(size, Path.GetFileNameWithoutExtension(file));

                if (!File.Exists(avatarFile))
                {
                    using (var image = Image.Load(file))
                    {
                        image.Mutate(ctx => ctx.ConvertToAvatar(s, s));
                        image.Save(avatarFile);
                    }
                }
                
                return avatarFile;
            });
        }

        private Task<string> CreateFallbackAvatar(long id, Size size)
        {
            return Task.Run(() =>
            {
                var index = Math.Abs(id) % _colors.Length;
                var color = _colors[index];
                var s = (int)size;

                var avatarFile = GetAvatarFilename(size, color);

                if (!File.Exists(avatarFile))
                {
                    using (var image = new Image<Rgba32>(s, s))
                    {
                        image.Mutate(ctx => ctx.BackgroundColor(Rgba32.FromHex(color)).ConvertToAvatar(s, s));
                        image.Save(avatarFile);
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

        private readonly string[] _colors =
        {
            "5caae9",
            "e66b66",
            "69cfbe",
            "c57fe1",
            "8ace7c",
            "f5b870"
        };

        public enum Size
        {
            Explorer = 40,
            Chat = 80
        }
    }
}