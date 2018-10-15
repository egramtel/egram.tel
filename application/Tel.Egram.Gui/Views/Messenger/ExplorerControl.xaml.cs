using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Markup.Xaml;
using ReactiveUI;

namespace Tel.Egram.Gui.Views.Messenger
{
    public class ExplorerControl : UserControl
    {
        public static readonly DirectProperty<ExplorerControl, Tuple<int, int>> VisibleIndexesProperty =
            AvaloniaProperty.RegisterDirect<ExplorerControl, Tuple<int, int>>(
                nameof(VisibleIndexes),
                o => o.VisibleIndexes,
                (o, v) => o.VisibleIndexes = v);
        
        private readonly ListBox _listBox;
        private IDisposable _scrollSubscription;

        public ExplorerControl()
        {
            AvaloniaXamlLoader.Load(this);
            
            _listBox = this.FindControl<ListBox>("ItemList");
        }

        private Tuple<int, int> _visibleIndexes;
        public Tuple<int, int> VisibleIndexes
        {
            get { return _visibleIndexes; }
            set { SetAndRaise(VisibleIndexesProperty, ref _visibleIndexes, value); }
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            var offsetChanges = _listBox.WhenAnyValue(lb => lb.Scroll.Offset)
                .Select(_ => Unit.Default);
            var extentChanges = _listBox.WhenAnyValue(lb => lb.Scroll.Extent)
                .Select(_ => Unit.Default);
            var viewportChanges = _listBox.WhenAnyValue(lb => lb.Scroll.Viewport)
                .Select(_ => Unit.Default);
            
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
                        VisibleIndexes = Tuple.Create(from, to);
                    }
                    else
                    {
                        VisibleIndexes = null;
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
