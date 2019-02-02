using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using ReactiveUI;
using Tel.Egram.Model.Messenger.Explorer;
using Tel.Egram.Model.Messenger.Homepage;
using Tel.Egram.Services.Utils;
using Tel.Egram.Services.Utils.Reactive;

namespace Tel.Egram.Views.Messenger.Homepage
{
    public class HomepageControl : BaseControl<HomepageModel>
    {   
        public HomepageControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
