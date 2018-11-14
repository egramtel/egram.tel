using Avalonia;
using Avalonia.LogicalTree;
using ReactiveUI;

namespace Tel.Egram.Gui.Views
{
    /// <summary>
    /// Hack around not working WhenActivated
    /// </summary>
    public class BaseControl<TViewModel> : ReactiveUserControl<TViewModel>
        where TViewModel : class
    {
        private object _dataContext;
        
        public BaseControl()
        {
            DataContextChanged += (sender, args) =>
            {   
                (_dataContext as ISupportsActivation)?.Activator?.Deactivate();
                (DataContext as ISupportsActivation)?.Activator?.Activate();
                _dataContext = DataContext;
            };
        }

        protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
        {
            base.OnAttachedToLogicalTree(e);
            (DataContext as ISupportsActivation)?.Activator?.Activate();
        }

        protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
        {
            (DataContext as ISupportsActivation)?.Activator?.Deactivate(true);
            base.OnDetachedFromLogicalTree(e);
        }
    }
}