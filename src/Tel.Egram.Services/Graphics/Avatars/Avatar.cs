using System;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace Tel.Egram.Services.Graphics.Avatars
{
    public class Avatar
    {
        public IBitmap Bitmap { get; set; }

        public Func<IBrush> BrushFactory { get; set; }
        
        public IBrush Brush => BrushFactory?.Invoke();
        
        public string Label { get; set; }

        public bool IsFallback => Bitmap == null;
    }
}