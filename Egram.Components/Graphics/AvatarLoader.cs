using System.IO;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Egram.Components.Persistence;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
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
        
        public async Task<IBitmap> LoadForUserAsync(TD.User user, Size size)
        {   
            var localFile = await _fileLoader.LoadFileAsync(user.ProfilePhoto?.Small);

            if (localFile?.Path != null)
            {
                var avatarFile = await CreateAvatar(localFile.Path, size);
                return await CreateBitmap(avatarFile);
            }

            return null;
        }

        public async Task<IBitmap> LoadForChatAsync(TD.Chat chat, Size size)
        {   
            var localFile = await _fileLoader.LoadFileAsync(chat.Photo?.Small);

            if (localFile?.Path != null)
            {
                var avatarFile = await CreateAvatar(localFile.Path, size);
                return await CreateBitmap(avatarFile);
            }

            return null;
        }

        private Task<string> CreateAvatar(string file, Size size)
        {
            return Task.Run(() =>
            {
                var thumbFile = Path.Combine(
                    _storage.AvatarCacheDirectory,
                    $"avatar_{(int)size}x{(int)size}_{Path.GetFileName(file)}");

                if (!File.Exists(thumbFile))
                {
                    using (var image = Image.Load(file))
                    {
                        image.Mutate(ctx => ctx.Resize((int)size, (int)size));
                        image.Save(thumbFile);
                    }
                }
                
                return thumbFile;
            });
        }

        private Task<Bitmap> CreateBitmap(string filePath)
        {
            return Task.Run(() => File.Exists(filePath) ? new Bitmap(filePath) : null);
        }

        public enum Size
        {
            Explorer = 40,
            Chat = 80
        }
    }
}