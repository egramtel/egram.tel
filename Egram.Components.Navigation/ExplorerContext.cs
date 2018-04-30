using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using Egram.Components.Navigation;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace Egram.Components.Navigation
{
    public class ExplorerContext : ReactiveObject, IDisposable
    {
        private readonly Scope _scope;
        private readonly AggregatorContextFactory _aggregatorContextFactory;
        private readonly CatalogContextFactory _catalogContextFactory;

        private readonly IDisposable _segmentSelectedSubscription;
        
        public ExplorerContext(
            Scope scope,
            AggregatorContextFactory aggregatorContextFactory,
            CatalogContextFactory catalogContextFactory
            )
        {
            _scope = scope;
            
            _aggregatorContextFactory = aggregatorContextFactory;
            _catalogContextFactory = catalogContextFactory;

            GoBackCommand = ReactiveCommand.Create(() => { SelectedExplorerIndex = 0; });
            
            AggregatorContext = _aggregatorContextFactory.FromScope(_scope);
            _segmentSelectedSubscription = Observable.FromEventPattern<Segment>(
                h => AggregatorContext.SegmentSelected += h,
                h => AggregatorContext.SegmentSelected -= h)
                .Select(e => e.EventArgs)
                .Subscribe(ObserveSelectedSegment);
        }

        private void ObserveSelectedSegment(Segment segment)
        {
            CatalogContext?.Dispose();
            CatalogContext = _catalogContextFactory.FromSegment(_scope, segment);

            AccessoryText = segment.Name;

            IsChannelSegment = segment.Kind.HasFlag(ExplorerEntityKind.Channel);
            IsGroupSegment = segment.Kind.HasFlag(ExplorerEntityKind.Group);
            IsBotSegment = segment.Kind.HasFlag(ExplorerEntityKind.Bot);
            IsDirectSegment = segment.Kind.HasFlag(ExplorerEntityKind.People);
            
            SelectedExplorerIndex = 1;
        }
        
        public ReactiveCommand GoBackCommand { get; }

        private string _accessoryText;
        public string AccessoryText
        {
            get => _accessoryText;
            set => this.RaiseAndSetIfChanged(ref _accessoryText, value);
        }

        private bool _isChannelSegment;
        public bool IsChannelSegment
        {
            get => _isChannelSegment;
            set => this.RaiseAndSetIfChanged(ref _isChannelSegment, value);
        }

        private bool _isGroupSegment;
        public bool IsGroupSegment
        {
            get => _isGroupSegment;
            set => this.RaiseAndSetIfChanged(ref _isGroupSegment, value);
        }
        
        private bool _isBotSegment;
        public bool IsBotSegment
        {
            get => _isBotSegment;
            set => this.RaiseAndSetIfChanged(ref _isBotSegment, value);
        }
        
        private bool _isDirectSegment;
        public bool IsDirectSegment
        {
            get => _isDirectSegment;
            set => this.RaiseAndSetIfChanged(ref _isDirectSegment, value);
        }
        
        private AggregatorContext _aggregatorContext;
        public AggregatorContext AggregatorContext
        {
            get => _aggregatorContext;
            set => this.RaiseAndSetIfChanged(ref _aggregatorContext, value);
        }

        private CatalogContext _catalogContext;
        public CatalogContext CatalogContext
        {
            get => _catalogContext;
            set => this.RaiseAndSetIfChanged(ref _catalogContext, value);
        }

        private int _selectedExplorerIndex = 0;
        public int SelectedExplorerIndex
        {
            get => _selectedExplorerIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedExplorerIndex, value);
        }

        public void Dispose()
        {
            _segmentSelectedSubscription?.Dispose();
        }
    }

    public class ExplorerContextFactory
    {
        private readonly IServiceProvider _provider;

        public ExplorerContextFactory(IServiceProvider provider)
        {
            _provider = provider;
        }
        
        public ExplorerContext FromScope(Scope scope)
        {
            return new ExplorerContext(
                scope,
                _provider.GetService<AggregatorContextFactory>(),
                _provider.GetService<CatalogContextFactory>());
        }
    }
}