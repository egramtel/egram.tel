using System;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace Tel.Egram.Services.Graphics.Avatars
{
    public class Avatar
    {
        public IBitmap Bitmap { get; set; }

        public Color TextColor { get; set; } = Colors.White;
        
        public Color Color { get; set; }
        
        public string Label { get; set; }

        public bool IsFallback => Bitmap == null;
    }
}