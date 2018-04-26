using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Egram.Gui.Animation;

namespace Egram.Gui.Navigation
{
    public class ExplorerControl : UserControl
    {
        private readonly Carousel _contentSlider;
        private readonly Carousel _toolbarSlider;

        public ExplorerControl()
        {
            AvaloniaXamlLoader.Load(this);

            _contentSlider = this.FindControl<Carousel>("ExplorerContentSlider");
            _contentSlider.Transition = new SlideTransition();

            _toolbarSlider = this.FindControl<Carousel>("ExplorerToolbarSlider");
            _toolbarSlider.Transition = new FadeTransition();
        }
    }
}
