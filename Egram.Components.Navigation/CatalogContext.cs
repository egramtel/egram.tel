using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace Egram.Components.Navigation
{
    public class CatalogContext : ReactiveObject, IDisposable
    {
        private readonly Scope _scope;
        private readonly Segment _segment;
        private readonly Navigator _navigator;
        private readonly SegmentInteractor _segmentInteractor;
        
        private readonly IDisposable _segmentFetchSubscription;
        private readonly IDisposable _explorerNavigationSubscription;

        public CatalogContext(
            Scope scope,
            Segment segment,
            Navigator navigator,
            SegmentInteractor segmentInteractor
            )
        {
            _scope = scope;
            _segment = segment;
            _navigator = navigator;
            _segmentInteractor = segmentInteractor;

            _explorerNavigationSubscription = this.WhenAnyValue(x => x.SelectedEntityIndex)
                .Subscribe(ObserveExplorerNavigation);
            
            _segmentFetchSubscription = _segmentInteractor
                .FetchByKind(segment.Kind ^ ExplorerEntityKind.Header)
                .SubscribeOn(Scheduler.Default)
                .Buffer(4)
                .ObserveOn(AvaloniaScheduler.Instance)
                .Subscribe(ObserveSegmentsFetch);
        }

        private void ObserveSegmentsFetch(IList<SegmentInteractor.Fetch> fetches)
        {
            var entities = new List<ExplorerEntity>();
            
            foreach (var fetch in fetches)
            {
                entities.AddRange(fetch.Targets);
            }
            
            Entities = new ReactiveList<ExplorerEntity>(entities);
        }

        private async void ObserveExplorerNavigation(int index)
        {
            if (_entities != null && index >= 0 && index < _entities.Count)
            {
                var entity = _entities[index];
                SelectedEntityIndex = index;

                await Task.Delay(250);
                
                var target = (SegmentTarget) entity;
                _navigator.Go(target.Topic);
            }
        }
        
        private ReactiveList<ExplorerEntity> _entities;
        public ReactiveList<ExplorerEntity> Entities
        {
            get => _entities;
            set => this.RaiseAndSetIfChanged(ref _entities, value);
        }

        private int _selectedEntityIndex = -1;
        public int SelectedEntityIndex
        {
            get => _selectedEntityIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedEntityIndex, value);
        }

        public void Dispose()
        {
            _segmentFetchSubscription?.Dispose();
            _explorerNavigationSubscription?.Dispose();
        }
    }

    public class CatalogContextFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CatalogContextFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public CatalogContext FromSegment(Scope scope, Segment segment)
        {
            return new CatalogContext(
                scope,
                segment,
                _serviceProvider.GetService<Navigator>(),
                _serviceProvider.GetService<SegmentInteractor>()
                );
        }
    }
}