using System;
using ReactiveUI;
using Tel.Egram.Components.Content;

namespace Tel.Egram.Components.Workspace
{
    public class NavigationInteractor
    {
        public NavigationInteractor()
        {
        }

        public IDisposable Bind(WorkspaceContext context)
        {
            return context.NavigationContext.WhenAnyValue(c => c.SelectedTabIndex)
                .Subscribe(index =>
                {
                    context.OnContentNavigation((ContentKind)index);
                });
        }
    }
}