using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Tel.Egram.Components.Content;
using Tel.Egram.Components.Explorer;
using Tel.Egram.Components.Navigation;
using Tel.Egram.Feeds;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Workspace
{
    public class WorkspaceContext : ReactiveObject, IDisposable
    {
        private readonly CompositeDisposable _contextDisposable = new CompositeDisposable();
        
        private readonly IFactory<NavigationContext> _navigationContextFactory;
        private readonly IFactory<ExplorerKind, ExplorerContext> _explorerContextFactory;
        private readonly IFactory<Target, ContentMessengerContext> _contentMessengerContextFactory;

        private NavigationContext _navigationContext;
        public NavigationContext NavigationContext
        {
            get => _navigationContext;
            set => this.RaiseAndSetIfChanged(ref _navigationContext, value);
        }

        private ExplorerContext _explorerContext;
        public ExplorerContext ExplorerContext
        {
            get => _explorerContext;
            set => this.RaiseAndSetIfChanged(ref _explorerContext, value);
        }

        private ContentContext _contentContext;
        public ContentContext ContentContext
        {
            get => _contentContext;
            set => this.RaiseAndSetIfChanged(ref _contentContext, value);
        }
        
        public WorkspaceContext(
            IFactory<NavigationContext> navigationContextFactory,
            IFactory<ExplorerKind, ExplorerContext> explorerContextFactory,
            IFactory<Target, ContentMessengerContext> contentMessengerContextFactory
            )
        {   
            _navigationContextFactory = navigationContextFactory;
            _explorerContextFactory = explorerContextFactory;
            _contentMessengerContextFactory = contentMessengerContextFactory;

            NavigationContext = _navigationContextFactory.Create();
            NavigationContext.WhenAnyValue(context => context.SelectedTabIndex)
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(AvaloniaScheduler.Instance)
                .Subscribe(HandleContentNavigation)
                .DisposeWith(_contextDisposable);
        }

        private void HandleContentNavigation(int index)
        {
            var explorerKind = (ExplorerKind) index;

            ContentContext?.Dispose();
            
            ExplorerContext?.Dispose();
            ExplorerContext = _explorerContextFactory.Create(explorerKind);
            ExplorerContext.WhenAnyValue(context => context.Target)
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(AvaloniaScheduler.Instance)
                .Subscribe(HandleTargetChange)
                .DisposeWith(_contextDisposable);
        }

        private void HandleTargetChange(Target target)
        {
            if (target == null)
            {
                return;
            }
            
            ContentContext?.Dispose();
            ContentContext = _contentMessengerContextFactory.Create(target);
        }
        
        public void Dispose()
        {
            NavigationContext?.Dispose();
            ExplorerContext?.Dispose();
            ContentContext?.Dispose();
            
            _contextDisposable.Dispose();
        }
    }
}