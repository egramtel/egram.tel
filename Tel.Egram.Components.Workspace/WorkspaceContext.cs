using System;
using Avalonia.Media.Imaging;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Tel.Egram.Components.Content;
using Tel.Egram.Components.Explorer;
using Tel.Egram.Components.Navigation;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Workspace
{
    public class WorkspaceContext : ReactiveObject, IDisposable
    {
        private readonly IFactory<ExplorerKind, ExplorerContext> _explorerContextFactory;
        private readonly IFactory<ContentKind, ContentContext> _contentContextFactory;
        private readonly NavigationInteractor _navigationInteractor;

        private readonly IDisposable _navigationSubscription;
        
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
            IFactory<ContentKind, ContentContext> contentContextFactory,
            IFactory<NavigationInteractor> navigationInteractorFactory
            )
        {
            _explorerContextFactory = explorerContextFactory;
            _contentContextFactory = contentContextFactory;
            
            NavigationContext = navigationContextFactory.Create();
            _navigationInteractor = navigationInteractorFactory.Create();
            
            _navigationSubscription = _navigationInteractor.Bind(this);
        }

        public void OnContentNavigation(ContentKind kind)
        {
            var contentKind = kind;
            var explorerKind = (ExplorerKind) kind;
            
            ExplorerContext = _explorerContextFactory.Create(explorerKind);
            ContentContext = _contentContextFactory.Create(contentKind);
        }

        public void Dispose()
        {
            _navigationSubscription.Dispose();
            
            NavigationContext?.Dispose();
            ExplorerContext?.Dispose();
            ContentContext?.Dispose();
        }
    }
}