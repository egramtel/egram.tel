using System;
using Avalonia.Media;

namespace Tel.Egram.Gui.Styles
{
    /// <summary>
    /// Temp workaround for using fonts in styles
    /// </summary>
    public class RobotoFontFamily : FontFamily
    {
        public RobotoFontFamily()
            : base ("Roboto", new Uri("resm:Tel.Egram.Gui.Fonts?assembly=Tel.Egram.Gui"))
        {
        }
    }
}