using Avalonia;
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
    }
}