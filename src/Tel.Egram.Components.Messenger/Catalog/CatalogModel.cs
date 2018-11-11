using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using DynamicData.Binding;
using PropertyChanged;
using ReactiveUI;
using Tel.Egram.Components.Messenger.Catalog.Entries;
using Tel.Egram.Messaging.Chats;

namespace Tel.Egram.Components.Messenger.Catalog
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
                this.BindProvider()
                    .DisposeWith(disposables);

                this.BindFilter(section)
                    .DisposeWith(disposables);
            });
        }

        private CatalogModel()
        {
        }
        
        public static CatalogModel Hidden()
        {
            return new CatalogModel
            {
                IsVisible = false
            };
        }

        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}