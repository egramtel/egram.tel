using System;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;

namespace Egram.Gui.Workareas
{
    public class SidepadControl : UserControl
    {
        public SidepadControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
