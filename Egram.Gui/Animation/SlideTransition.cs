using System;
using Avalonia.Animation;

namespace Egram.Gui.Animation
{
    public class SlideTransition : PageSlide
    {
        public SlideTransition()
            : base (TimeSpan.FromSeconds(0.25), SlideAxis.Horizontal)
        {
            
        }
    }
}