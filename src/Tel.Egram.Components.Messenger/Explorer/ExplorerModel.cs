using System.Reactive.Disposables;
using DynamicData;
using DynamicData.Binding;
using PropertyChanged;
using ReactiveUI;
using Tel.Egram.Components.Messenger.Explorer.Items;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Messenger.Explorer
{
    [AddINotifyPropertyChangedInterface]
    public class ExplorerModel : ISupportsActivation
    {
        public bool IsVisible { get; set; } = true;
        
        public ObservableCollectionExtended<ItemModel> Items { get; set; }
            = new ObservableCollectionExtended<ItemModel>();
        
        public SourceList<ItemModel> SourceItems { get; set; }
            = new SourceList<ItemModel>();
        
        public Range VisibleRange { get; set; }
        
        public ExplorerModel(Target target)
        {
            this.WhenActivated(disposables =>
            {
                this.BindSource()
                    .DisposeWith(disposables);
            
                this.BindVisibleRangeChanges(target)
                    .DisposeWith(disposables);
            
                this.InitMessageLoading(target)
                    .DisposeWith(disposables);
            });
        }

        private ExplorerModel()
        {
        }
        
        public static ExplorerModel Hidden()
        {
            return new ExplorerModel
            {
                IsVisible = false
            };
        }
        
        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}