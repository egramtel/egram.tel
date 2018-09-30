using System;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace Tel.Egram.Graphics
{
    public class Avatar
    {
        public IBitmap Bitmap { get; set; }

        public Func<IBrush> BrushFactory { get; set; }
        
        public IBrush Brush => BrushFactory?.Invoke();
        
        public string Label { get; set; }
        
        public AvatarSize Size { get; set; }

        public bool IsFallback => Bitmap == null;
    }
}