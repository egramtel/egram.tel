using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using ReactiveUI;
using Tel.Egram.Model.Messenger.Explorer;
using Tel.Egram.Services.Utils;
using Tel.Egram.Services.Utils.Reactive;

namespace Tel.Egram.Views.Messenger.Explorer
{
    public class ExplorerControl : BaseControl<ExplorerModel>
    {
        public static readonly DirectProperty<ExplorerControl, Range> VisibleRangeProperty =
            AvaloniaProperty.RegisterDirect<ExplorerControl, Range>(
                nameof(VisibleRange),
                o => o.VisibleRange,
                (o, v) => o.VisibleRange = v);

        public static readonly DirectProperty<ExplorerControl, object> TargetItemProperty =
            AvaloniaProperty.RegisterDirect<ExplorerControl, object>(
                nameof(TargetItem),
                o => o.TargetItem,
                (o, v) => o.TargetItem = v);

        private readonly ListBox _listBox;
        
        public ExplorerControl()
        {
            AvaloniaXamlLoader.Load(this);
            
            _listBox = this.FindControl<ListBox>("ItemList");
            
            this.WhenActivated(disposables =>
            {
                var offsetChanges = _listBox.WhenAnyValue(lb => lb.Scroll.Offset)
                    .Select(_ => Unit.Default);
                var extentChanges = _listBox.WhenAnyValue(lb => lb.Scroll.Extent)
                    .Select(_ => Unit.Default);
                var viewportChanges = _listBox.WhenAnyValue(lb => lb.Scroll.Viewport)
                    .Select(_ => Unit.Default);
                
                offsetChanges
                    .Merge(extentChanges)
                    .Merge(viewportChanges)
                    .Accept(_ => HandleOffsetChange())
                    .DisposeWith(disposables);

                this.WhenAnyValue(x => x.TargetItem)
                    .Accept(item => HandleTargetItem(item))
                    .DisposeWith(disposables);
            });
        }

        private void HandleTargetItem(object item)
        {
            if (item != null)
            {
                RxApp.MainThreadScheduler.Schedule(
                    "",
                    TimeSpan.FromSeconds(0.1),
                    (scheduler, s) =>
                    {
                        _listBox.ScrollIntoView(item);
                        return Disposable.Empty;
                    });
            }
        }

        private void HandleOffsetChange()
        {
            var offset = _listBox.Scroll.Offset;
            var viewport = _listBox.Scroll.Viewport;

            var topTreshold = offset.Y;
            var bottomTreshold = offset.Y + viewport.Height;
                
            int i = 0;
            var items = _listBox?.Items;

            int from = Int32.MaxValue;
            int to = Int32.MinValue;
                
            if (items != null)
            {
                foreach (var item in items)
                {
                    var listBoxItem = (ListBoxItem)_listBox.ItemContainerGenerator.ContainerFromIndex(i);
                        
                    var top = listBoxItem.Bounds.TopLeft.Y;
                    var bottom = listBoxItem.Bounds.BottomLeft.Y;
                    if (bottom >= topTreshold && top <= bottomTreshold)
                    {
                        if (i < from)
                        {
                            from = i;
                        }

                        if (i > to)
                        {
                            to = i;
                        }
                    }
                    i++;
                }
            }

            if (from != Int32.MaxValue && to != Int32.MinValue)
            {
                VisibleRange = new Range(from, to - from + 1);
            }
            else
            {
                VisibleRange = default(Range);
            }
        }

        private Range _visibleRange;
        public Range VisibleRange
        {
            get => _visibleRange;
            set => SetAndRaise(VisibleRangeProperty, ref _visibleRange, value);
        }

        private object _targetItem;
        public object TargetItem
        {
            get => _targetItem;
            set => SetAndRaise(TargetItemProperty, ref _targetItem, value);
        }
    }
}
