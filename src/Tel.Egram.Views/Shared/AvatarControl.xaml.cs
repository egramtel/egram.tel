using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace Tel.Egram.Views.Shared
{
    public class AvatarControl : ContentControl
    {
        public static readonly StyledProperty<IBitmap> SourceProperty =
            AvaloniaProperty.Register<AvatarControl, IBitmap>(nameof(Source));

        public static readonly StyledProperty<IBrush> SourceBrushProperty =
            AvaloniaProperty.Register<AvatarControl, IBrush>(nameof(SourceBrush));

        public static readonly StyledProperty<Color> ColorProperty =
            AvaloniaProperty.Register<AvatarControl, Color>(nameof(Color));

        public static readonly StyledProperty<IBrush> ColorBrushProperty =
            AvaloniaProperty.Register<AvatarControl, IBrush>(nameof(ColorBrush));

        public static readonly StyledProperty<string> TextProperty =
            AvaloniaProperty.Register<AvatarControl, string>(nameof(Text));

        public static readonly StyledProperty<Color> TextColorProperty =
            AvaloniaProperty.Register<AvatarControl, Color>(nameof(TextColor));

        public static readonly StyledProperty<IBrush> TextColorBrushProperty =
            AvaloniaProperty.Register<AvatarControl, IBrush>(nameof(TextColorBrush));

        public static readonly StyledProperty<double> SizeProperty =
            AvaloniaProperty.Register<AvatarControl, double>(nameof(Size));
        
        public AvatarControl()
        {
            AvaloniaXamlLoader.Load(this);

            SourceProperty.Changed.Subscribe(SourceChanged);
            ColorProperty.Changed.Subscribe(ColorChanged);
            TextColorProperty.Changed.Subscribe(TextColorChanged);
        }
        
        public IBitmap Source
        {
            get => GetValue(SourceProperty);
            set
            {
                SetValue(SourceProperty, value);
                SetValue(SourceBrushProperty, new ImageBrush(value));
            }
        }

        public IBrush SourceBrush
        {
            get => GetValue(SourceBrushProperty);
            set => SetValue(SourceBrushProperty, value);
        }

        public Color Color
        {
            get => GetValue(ColorProperty);
            set
            {
                SetValue(ColorProperty, value);
                SetValue(ColorBrushProperty, new SolidColorBrush(value));
            }
        }

        public IBrush ColorBrush
        {
            get => GetValue(ColorBrushProperty);
            set => SetValue(ColorBrushProperty, value);
        }

        public string Text
        {
            get => GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public Color TextColor
        {
            get => GetValue(TextColorProperty);
            set
            {
                SetValue(TextColorProperty, value);
                SetValue(TextColorBrushProperty, new SolidColorBrush(value));
            }
        }

        public IBrush TextColorBrush
        {
            get => GetValue(TextColorBrushProperty);
            set => SetValue(TextColorBrushProperty, value);
        }

        public double Size
        {
            get => GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        private static void SourceChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Sender is AvatarControl avatarControl)
            {
                if (e.NewValue is null)
                {
                    avatarControl.SourceBrush = null;
                }
                
                if (e.NewValue is IBitmap bitmap)
                {
                    avatarControl.SourceBrush = new ImageBrush(bitmap);
                }
            }
        }
        
        private static void ColorChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Sender is AvatarControl avatarControl)
            {
                if (e.NewValue is null)
                {
                    avatarControl.ColorBrush = null;
                }
                
                if (e.NewValue is Color color)
                {
                    avatarControl.ColorBrush = new SolidColorBrush(color);
                }
            }
        }
        
        private static void TextColorChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Sender is AvatarControl avatarControl)
            {
                if (e.NewValue is null)
                {
                    avatarControl.TextColorBrush = null;
                }
                
                if (e.NewValue is Color color)
                {
                    avatarControl.TextColorBrush = new SolidColorBrush(color);
                }
            }
        }
    }
}
