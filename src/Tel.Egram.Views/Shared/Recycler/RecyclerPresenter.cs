using System;
using System.Collections.Generic;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;

namespace Tel.Egram.Views.Shared.Recycler
{
    public class RecyclerPresenter : Control, ILogicalScrollable
    {
        public RecyclerPresenter()
        {
            Extent = new Size(240, 640);
            Offset = new Vector(0, 160);
            Viewport = new Size(240, 320);
        }

        public Size Extent { get; }

        public Vector Offset { get; set; }

        public Size Viewport { get; }

        public Size ScrollSize { get; } = new Size(1, 1);

        public Size PageScrollSize { get; } = new Size(0, 1);

        public bool IsLogicalScrollEnabled { get; } = true;

        public bool CanHorizontallyScroll { get; set; }

        public bool CanVerticallyScroll { get; set; }

        public Action InvalidateScroll { get; set; }

        public RecyclerPanel Panel { get; private set; }

        public override sealed void ApplyTemplate()
        {
            Panel = new RecyclerPanel();
            Panel.SetValue(TemplatedParentProperty, TemplatedParent);

            LogicalChildren.Clear();
            VisualChildren.Clear();
            LogicalChildren.Add(Panel);
            VisualChildren.Add(Panel);
            
            InvalidateScroll?.Invoke();
        }

        public bool BringIntoView(IControl target, Rect targetRect)
        {
            return false;
        }

        public IControl GetControlInDirection(NavigationDirection direction, IControl from)
        {
            return null;
        }
    }
}
