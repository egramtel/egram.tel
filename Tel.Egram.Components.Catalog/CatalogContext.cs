using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Tel.Egram.Feeds;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Catalog
{
    public class CatalogContext : ReactiveObject, IDisposable
    {
        private readonly CatalogInteractor _catalogInteractor;
        
        private readonly IDisposable _catalogLoadingSubscription;

        public CatalogContext(
            CatalogKind kind,
            IFactory<CatalogInteractor> catalogInteractorFactory
            )
        {
            _catalogInteractor = catalogInteractorFactory.Create();
            
            _catalogLoadingSubscription = _catalogInteractor.LoadCatalog(this, kind);
        }
        
        private SectionModel _section;
        public SectionModel Section
        {
            get => _section;
            set => this.RaiseAndSetIfChanged(ref _section, value);
        }

        public void OnSectionLoaded(SectionModel section)
        {
            foreach (var chatEntry in section.Entries)
            {
                chatEntry.LoadAvatar();
            }

            Section = section;
        }

        public void Dispose()
        {
            _catalogLoadingSubscription.Dispose();
        }
    }
}