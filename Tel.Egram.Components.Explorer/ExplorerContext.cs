using System;
using ReactiveUI;
using Tel.Egram.Components.Catalog;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Explorer
{
    public class ExplorerContext : ReactiveObject, IDisposable
    {
        private CatalogContext _catalogContext;
        public CatalogContext CatalogContext
        {
            get => _catalogContext;
            set => this.RaiseAndSetIfChanged(ref _catalogContext, value);
        }

        public ExplorerContext(
            ExplorerKind explorerKind,
            IFactory<CatalogKind, CatalogContext> catalogContextFactory
            )
        {
            var sectionKind = (CatalogKind) explorerKind;
            CatalogContext = catalogContextFactory.Create(sectionKind);
        }

        public void Dispose()
        {
            
        }
    }
}