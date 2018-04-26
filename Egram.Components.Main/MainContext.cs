using System;
using Egram.Components.Navigation;
using Egram.Components.Workarea;
using Egram.Components.Navigation;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace Egram.Components.Main
{
    public class MainContext : ReactiveObject, IDisposable
    {
        private readonly Navigator _navigator;
        
        private readonly ExplorerContextFactory _explorerContextFactory;
        private readonly WorkareaContextFactory _workareaContextFactory;

        private readonly IDisposable _scopeNavigationSubscription;
        private readonly IDisposable _topicNavigationSubscription;

        public MainContext(
            Navigator navigator,
            ExplorerContextFactory explorerContextFactory,
            WorkareaContextFactory workareaContextFactory)
        {
            _navigator = navigator;
            
            _explorerContextFactory = explorerContextFactory;
            _workareaContextFactory = workareaContextFactory;
            
            _scopeNavigationSubscription = _navigator
                .ScopeNavigations()
                .Subscribe(ObserveScopeNavigation);

            _topicNavigationSubscription = _navigator
                .TopicNavigations()
                .Subscribe(ObserveTopicNavigation);

            _workareaContext = _workareaContextFactory.CreateEmpty();
            
            navigator.Go(new Scope());
        }
        
        private ExplorerContext _explorerContext;
        public ExplorerContext ExplorerContext
        {
            get => _explorerContext;
            set => this.RaiseAndSetIfChanged(ref _explorerContext, value);
        }
        
        private WorkareaContext _workareaContext;
        public WorkareaContext WorkareaContext
        {
            get => _workareaContext;
            set => this.RaiseAndSetIfChanged(ref _workareaContext, value);
        }

        private void ObserveScopeNavigation(Scope scope)
        {
            ExplorerContext?.Dispose();
            ExplorerContext = _explorerContextFactory.FromScope(scope);
        }

        private void ObserveTopicNavigation(Topic topic)
        {
            WorkareaContext?.Dispose();
            WorkareaContext = _workareaContextFactory.FromTopic(topic);
        }
        
        public void Dispose()
        {
            _scopeNavigationSubscription?.Dispose();
            _topicNavigationSubscription?.Dispose();
        }
    }

    public class MainContextFactory
    {
        private readonly IServiceProvider _provider;

        public MainContextFactory(IServiceProvider provider)
        {
            _provider = provider;
        }
        
        public MainContext Create()
        {
            return _provider.GetService<MainContext>();
        }
    }
}