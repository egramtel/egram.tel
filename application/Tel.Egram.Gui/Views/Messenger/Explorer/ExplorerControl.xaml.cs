using System;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using Tel.Egram.Utils;

namespace Tel.Egram.Gui.Views.Messenger.Explorer
{
    public class ExplorerControl : UserControl
    {
        public static readonly DirectProperty<ExplorerControl, Range> VisibleIndexesProperty =
            AvaloniaProperty.RegisterDirect<ExplorerControl, Range>(
                nameof(VisibleRange),
                o => o.VisibleRange,
                (o, v) => o.VisibleRange = v);
        
        private readonly ListBox _listBox;
        private IDisposable _scrollSubscription;

        public ExplorerControl()
        {
            AvaloniaXamlLoader.Load(this);
            
            _listBox = this.FindControl<ListBox>("ItemList");
        }

        private Range _visibleRange;
        public Range VisibleRange
        {
            get { return _visibleRange; }
            set { SetAndRaise(VisibleIndexesProperty, ref _visibleRange, value); }
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            var offsetChanges = _listBox.WhenAnyValue(lb => lb.Scroll.Offset)
                .Select(_ => Unit.Default);
            var extentChanges = _listBox.WhenAnyValue(lb => lb.Scroll.Extent)
                .Select(_ => Unit.Default);
            var viewportChanges = _listBox.WhenAnyValue(lb => lb.Scroll.Viewport)
                .Select(_ => Unit.Default);
            
            // TODO: more efficient algorithm
            _scrollSubscription = offsetChanges
                .Merge(extentChanges)
                .Merge(viewportChanges)
                .Subscribe(_ =>
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
                });
            
            base.OnAttachedToVisualTree(e);
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            _scrollSubscription.Dispose();
            
            base.OnDetachedFromVisualTree(e);
        }
    }
}
