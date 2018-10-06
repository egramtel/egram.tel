using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Threading;
using PropertyChanged;
using ReactiveUI;
using Tel.Egram.Components.Catalog;
using Tel.Egram.Feeds;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Explorer
{
    [AddINotifyPropertyChangedInterface]
    public class ExplorerContext : IDisposable
    {
        private readonly CompositeDisposable _contextDisposable = new CompositeDisposable();
        
        public CatalogContext CatalogContext { get; set; }
        public Target Target { get; set; }

        public ExplorerContext(
            IFactory<CatalogKind, CatalogContext> catalogContextFactory,
            ExplorerKind explorerKind)
        {
            var catalogKind = (CatalogKind) explorerKind;
            
            CatalogContext = catalogContextFactory.Create(catalogKind);
            CatalogContext.WhenAnyValue(context => context.SelectedEntry)
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(AvaloniaScheduler.Instance)
                .Subscribe(HandleSelectedEntry)
                .DisposeWith(_contextDisposable);
        }

        private void HandleSelectedEntry(EntryModel entry)
        {
            switch (entry)
            {
                case EntryModelProxy modelProxy:
                    HandleSelectedEntry(modelProxy.EntryModel);
                    break;
                
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