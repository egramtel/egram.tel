using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Threading;
using PropertyChanged;
using ReactiveUI;
using Tel.Egram.Components.Messenger.Catalog;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Workspace
{
    [AddINotifyPropertyChangedInterface]
    public class WorkspaceContext : IDisposable
    {
        private readonly CompositeDisposable _contextDisposable = new CompositeDisposable();
        private readonly IFactory<Target, ContentMessengerContext> _contentMessengerContextFactory;
        private readonly IFactory<ExplorerKind, ExplorerContext> _explorerContextFactory;
        private readonly IFactory<NavigationContext> _navigationContextFactory;

        public NavigationContext NavigationContext { get; set; }
        public ExplorerContext ExplorerContext { get; set; }
        public ContentContext ContentContext { get; set; }
        
        public WorkspaceContext(
            IFactory<Target, ContentMessengerContext> contentMessengerContextFactory,
            IFactory<ExplorerKind, ExplorerContext> explorerContextFactory,
            IFactory<NavigationContext> navigationContextFactory)
        {   
            _contentMessengerContextFactory = contentMessengerContextFactory;
            _navigationContextFactory = navigationContextFactory;
            _explorerContextFactory = explorerContextFactory;

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