using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;

namespace Egram.Gui.Navigation
{
    public class AggregatorControl : UserControl
    {
        public AggregatorControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
