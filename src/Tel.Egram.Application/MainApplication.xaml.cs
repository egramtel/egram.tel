using System;
using Avalonia;
using Avalonia.Markup.Xaml;

namespace Tel.Egram.Application
{
    public class MainApplication : Avalonia.Application
    {
        public event EventHandler Initializing;

        public event EventHandler Disposing;
        
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            
            Initializing?.Invoke(this, null);
        }
        
        protected override void OnExiting(object sender, EventArgs e)
        {
            Disposing?.Invoke(this, null);
            
            base.OnExiting(sender, e);
        }
    }
}
