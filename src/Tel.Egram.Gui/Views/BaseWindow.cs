using Avalonia;
using ReactiveUI;

namespace Tel.Egram.Gui.Views
{
    /// <summary>
    /// Hack around not working WhenActivated
    /// </summary>
    public class BaseWindow<TViewModel> : ReactiveWindow<TViewModel>
        where TViewModel : class
    {
        private object _dataContext;
        
        public BaseWindow()
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