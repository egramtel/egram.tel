using System;
using Avalonia.Animation;

namespace Egram.Gui.Animation
{
    public class FadeTransition : CrossFade
    {
        public FadeTransition()
            : base (TimeSpan.FromSeconds(0.25))
        {
            
        }
    }
}