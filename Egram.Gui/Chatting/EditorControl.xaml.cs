using System;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Egram.Gui.Chatting
{
    public class EditorControl : UserControl
    {
        private readonly TextBox _messageEditorTextBox;
        private readonly StackPanel _messageEditorToolbar;

        public EditorControl()
        {
            AvaloniaXamlLoader.Load(this);
            
            _messageEditorTextBox = this.Find<TextBox>("MessageEditorTextBox");
            _messageEditorTextBox.GotFocus += OnGotFocus;
            _messageEditorTextBox.LostFocus += OnLostFocus;

            _messageEditorToolbar = this.Find<StackPanel>("MessageEditorToolbar");
        }

        private void OnGotFocus(object sender, RoutedEventArgs args)
        {   
            var minHeightAnimation = Animate.Property(
                _messageEditorTextBox,
                MinHeightProperty,
                0,
                50,
                new LinearDoubleEasing(),
                TimeSpan.FromMilliseconds(100)
                );
            
            minHeightAnimation.Subscribe(
                h => { },
                e => { },
                () => { _messageEditorTextBox.MinHeight = 50; });
        }

        private void OnLostFocus(object sender, RoutedEventArgs args)
        {   
            var minHeightAnimation = Animate.Property(
                _messageEditorTextBox,
                MinHeightProperty,
                50,
                0,
                new LinearDoubleEasing(),
                TimeSpan.FromMilliseconds(100)
                );
            
            minHeightAnimation.Subscribe(
                h => { },
                e => { },
                () => { _messageEditorTextBox.MinHeight = 0; });
        }
    }
}
