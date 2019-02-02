using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using DynamicData.Binding;
using PropertyChanged;
using ReactiveUI;
using Tel.Egram.Model.Messenger.Catalog.Entries;
using Tel.Egram.Services.Messaging.Chats;

namespace Tel.Egram.Model.Messenger.Catalog
{
    [AddINotifyPropertyChangedInterface]
    public class CatalogModel : ISupportsActivation
    {
        public bool IsVisible { get; set; } = true;
        
        public EntryModel SelectedEntry { get; set; }
        
        public ObservableCollectionExtended<EntryModel> Entries { get; set; }
            = new ObservableCollectionExtended<EntryModel>();
        
        public string SearchText { get; set; }
        
        public Subject<IComparer<EntryModel>> SortingController { get; set; }
            = new Subject<IComparer<EntryModel>>();
        
        public Subject<Func<EntryModel, bool>> FilterController { get; set; }
            = new Subject<Func<EntryModel, bool>>();

        public CatalogModel(Section section)
        {
            this.WhenActivated(disposables =>
            {
                new CatalogFilter()
                    .Bind(this, section)
                    .DisposeWith(disposables);
                
                new CatalogProvider()
                    .Bind(this, section)
                    .DisposeWith(disposables);
            });
        }

        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}