using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Threading;
using ReactiveUI;
using Tel.Egram.Components.Catalog;
using Tel.Egram.Feeds;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Explorer
{
    public class ExplorerContext : ReactiveObject, IDisposable
    {
        private readonly CompositeDisposable _contextDisposable = new CompositeDisposable();
        
        private CatalogContext _catalogContext;
        public CatalogContext CatalogContext
        {
            get => _catalogContext;
            set => this.RaiseAndSetIfChanged(ref _catalogContext, value);
        }

        private Target _target;
        public Target Target
        {
            get => _target;
            set => this.RaiseAndSetIfChanged(ref _target, value);
        }

        public ExplorerContext(
            ExplorerKind explorerKind,
            IFactory<CatalogKind, CatalogContext> catalogContextFactory
            )
        {
            var catalogKind = (CatalogKind) explorerKind;
            
            CatalogContext = catalogContextFactory.Create(catalogKind);
            CatalogContext.WhenAnyValue(context => context.SelectedEntryIndex)
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(AvaloniaScheduler.Instance)
                .Subscribe(HandleSelectedEntry)
                .DisposeWith(_contextDisposable);
        }

        private void HandleSelectedEntry(int index)
        {
            if (CatalogContext.Entries == null || CatalogContext.Entries.Count == 0)
            {
                return;
            }
            
            var entry = CatalogContext.Entries[index];

            switch (entry)
            {
                case AggregateEntryModel aggregateEntry:
                    Target = aggregateEntry.Aggregate;
                    break;
                
                case ChatEntryModel chatEntry:
                    Target = chatEntry.Chat;
                    break;
            }
        }

        public void Dispose()
        {
            CatalogContext?.Dispose();
            
            _contextDisposable.Dispose();
        }
    }
}