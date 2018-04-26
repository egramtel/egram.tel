using System;
using Avalonia;
using Avalonia.Markup.Xaml;

namespace Egram.Components.App
{
    public class App : Application
    {
        private readonly BackgroundWorker _backgroundWorker;
        private IDisposable _backgroundJobs;

        public App(BackgroundWorker backgroundWorker)
        {
            _backgroundWorker = backgroundWorker;
        }
        
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);

            _backgroundJobs = _backgroundWorker.Init();
        }
        
        protected override void OnExiting(object sender, EventArgs e)
        {
            _backgroundJobs?.Dispose();
            
            base.OnExiting(sender, e);
        }
    }
}
