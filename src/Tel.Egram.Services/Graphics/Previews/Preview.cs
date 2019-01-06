using Avalonia.Media.Imaging;

namespace Tel.Egram.Services.Graphics.Previews
{
    public class Preview
    {
        public IBitmap Bitmap { get; set; }
        
        public PreviewQuality Quality { get; set; }
    }
}