using System;
using PropertyChanged;

namespace Tel.Egram.Components.Content
{
    [AddINotifyPropertyChangedInterface]
    public abstract class ContentContext : IDisposable
    {
        private int _selectedIndex = -1;
        public int SelectedIndex { get; set; }
        
        public ContentContext(ContentKind kind)
        {
            SelectedIndex = (int) kind;
        }

        public abstract void Dispose();
    }
}