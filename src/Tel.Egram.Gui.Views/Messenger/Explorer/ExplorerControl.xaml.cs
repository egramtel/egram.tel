using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using Tel.Egram.Model.Messenger.Explorer;
using Tel.Egram.Services.Utils;
using Tel.Egram.Services.Utils.Reactive;

namespace Tel.Egram.Gui.Views.Messenger.Explorer
{
    public class ExplorerControl : BaseControl<ExplorerModel>
    {
        public static readonly DirectProperty<ExplorerControl, Range> VisibleRangeProperty =
            AvaloniaProperty.RegisterDirect<ExplorerControl, Range>(
                nameof(VisibleRange),
                o => o.VisibleRange,
                (o, v) => o.VisibleRange = v);

        public ExplorerControl()
        {
            AvaloniaXamlLoader.Load(this);
            
            var listBox = this.FindControl<ListBox>("ItemList");
            
            this.WhenActivated(disposables =>
            {
                var offsetChanges = listBox.WhenAnyValue(lb => lb.Scroll.Offset)
                    .Select(_ => Unit.Default);
                var extentChanges = listBox.WhenAnyValue(lb => lb.Scroll.Extent)
                    .Select(_ => Unit.Default);
                var viewportChanges = listBox.WhenAnyValue(lb => lb.Scroll.Viewport)
                    .Select(_ => Unit.Default);
                
                offsetChanges
                    .Merge(extentChanges)
                    .Merge(viewportChanges)
                    .Accept(_ =>
                    {
                        var offset = listBox.Scroll.Offset;
                        var viewport = listBox.Scroll.Viewport;

                        var topTreshold = offset.Y;
                        var bottomTreshold = offset.Y + viewport.Height;
                    
                        int i = 0;
                        var items = listBox?.Items;

                        int from = Int32.MaxValue;
                        int to = Int32.MinValue;
                    
                        if (items != null)
                        {
                            foreach (var item in items)
                            {
                                var listBoxItem = (ListBoxItem)listBox.ItemContainerGenerator.ContainerFromIndex(i);
                            
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
                    })
                    .DisposeWith(disposables);
            });
        }

        private Range _visibleRange;
        public Range VisibleRange
        {
            get { return _visibleRange; }
            set { SetAndRaise(VisibleRangeProperty, ref _visibleRange, value); }
        }
    }
}
