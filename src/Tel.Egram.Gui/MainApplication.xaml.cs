using System;
using Avalonia.Markup.Xaml;

namespace Tel.Egram.Gui
{
    public class MainApplication : Avalonia.Application
    {
        private readonly Initializer _initializer;

        public MainApplication(Initializer initializer)
        {
            _initializer = initializer;
        }
        
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            _initializer.Init();
        }
        
        protected override void OnExiting(object sender, EventArgs e)
        {
            _initializer.Dispose();
            base.OnExiting(sender, e);
        }
        
        public class Initializer : IDisposable
        {
            private Func<IDisposable> _init;
            private IDisposable _handle;

            public Initializer(Func<IDisposable> init)
            {
                _init = init;
            }

            public void Init()
            {
                _handle = _init();
            }

            public void Dispose()
            {
                _handle?.Dispose();
            }
        }
    }
}
