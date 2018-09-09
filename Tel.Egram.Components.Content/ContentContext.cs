using System;
using ReactiveUI;

namespace Tel.Egram.Components.Content
{
    public abstract class ContentContext : ReactiveObject, IDisposable
    {
        private int _selectedIndex = -1;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedIndex, value);
        }
        
        public ContentContext(ContentKind kind)
        {
            SelectedIndex = (int) kind;
        }

        public abstract void Dispose();
    }
}