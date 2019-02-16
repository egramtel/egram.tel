using System;
using System.Collections.Generic;
using System.Text;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;

namespace Tel.Egram.Views.Shared.Recycler
{
    public class RecyclerView : TemplatedControl
    {
        public RecyclerView()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
