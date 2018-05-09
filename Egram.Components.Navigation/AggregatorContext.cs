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
    public class AggregatorContext : ReactiveObject, IDisposable
    {
        private readonly Scope _scope;
        private readonly Navigator _navigator;
        private readonly SegmentInteractor _segmentInteractor;
        
        private readonly IDisposable _segmentFetchSubscription;
        private readonly IDisposable _explorerNavigationSubscription;
        
        public AggregatorContext(
            Scope scope,
            Navigator navigator,
            SegmentInteractor segmentInteractor
            )
        {
            _scope = scope;
            _navigator = navigator;
            _segmentInteractor = segmentInteractor;

            _entities = new ReactiveList<ExplorerEntity>();
            
            _explorerNavigationSubscription = this.WhenAnyValue(x => x.SelectedEntityIndex)
                .Subscribe(ObserveExplorerNavigation);
            
            _segmentFetchSubscription = _segmentInteractor
                .FetchAggregated()
                .SubscribeOn(Scheduler.Default)
                .ObserveOn(AvaloniaScheduler.Instance)
                .Subscribe(ObserveSegmentsFetch);
        }

        private void ObserveSegmentsFetch(SegmentInteractor.Fetch fetch)
        {
            _entities.Add(fetch.Segment);
            _entities.AddRange(fetch.Conversations);
            
            fetch.Updates
                .Buffer(TimeSpan.FromMilliseconds(100))
                .ObserveOn(AvaloniaScheduler.Instance)
                .Subscribe(updates =>
                {
                    foreach (var update in updates)
                    {
                        update.Conversation.Avatar = update.Avatar;
                    }
                });
        }

        private int _prevIndex = -1;
        private async void ObserveExplorerNavigation(int index)
        {
            if (_entities != null && index >= 0 && index < _entities.Count)
            {
                var entity = _entities[index];
                if (entity.IsHeader)
                {
                    // HACK: do not allow header selection
                    await Task.Delay(1);
                    SelectedEntityIndex = _prevIndex;

                    var segment = (Segment) entity;
                    SegmentSelected?.Invoke(this, segment);
                }
                else if (index != _prevIndex)
                {
                    SelectedEntityIndex = index;
                    _prevIndex = index;

                    await Task.Delay(250);
                    
                    var conversation = (Conversation) entity;
                    _navigator.Go(new Topic(conversation.Chat));
                }
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

        public event EventHandler<Segment> SegmentSelected;
        
        public void Dispose()
        {
            _segmentFetchSubscription?.Dispose();
            _explorerNavigationSubscription?.Dispose();
        }
    }

    public class AggregatorContextFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public AggregatorContextFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public AggregatorContext FromScope(Scope scope)
        {
            return new AggregatorContext(
                scope,
                _serviceProvider.GetService<Navigator>(),
                _serviceProvider.GetService<SegmentInteractor>()
                );
        }
    }
}