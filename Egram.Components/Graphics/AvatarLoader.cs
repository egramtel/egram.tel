using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Egram.Components.Persistence;

namespace Egram.Components.Graphics
{
    public class AvatarLoader
    {
        private readonly FileLoader _fileLoader;

        public AvatarLoader(FileLoader fileLoader)
        {
            _fileLoader = fileLoader;
        }
        
        public async Task<IBitmap> LoadForUserAsync(TD.User user)
        {
            var localFile = await _fileLoader.LoadFileAsync(user.ProfilePhoto?.Small);

            if (localFile?.Path != null)
            {
                return await CreateBitmap(localFile.Path);
            }

            return null;
        }

        public async Task<IBitmap> LoadForChatAsync(TD.Chat chat)
        {   
            var localFile = await _fileLoader.LoadFileAsync(chat.Photo?.Small);

            if (localFile?.Path != null)
            {
                return await CreateBitmap(localFile.Path);
            }

            return null;
        }

        private Task<Bitmap> CreateBitmap(string filePath)
        {
            return Task.Run(() => new Bitmap(filePath));
        }
    }
}