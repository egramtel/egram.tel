using System;
using System.Linq.Expressions;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;

namespace Tel.Egram.Components.Catalog
{
    public class EntryModelProxy : EntryModel, IDisposable
    {
        private readonly CompositeDisposable _modelDisposable = new CompositeDisposable();
        
        public EntryModelProxy(EntryModel entryModel)
        {
            EntryModel = entryModel;

            BindProperty(m => m.Id);
            BindProperty(m => m.Order);
            BindProperty(m => m.Title);
            BindProperty(m => m.Avatar);
            BindProperty(m => m.HasUnread);
            BindProperty(m => m.UnreadCount);
        }

        private void BindProperty<T>(Expression<Func<EntryModel, T>> property)
        {
            EntryModel.WhenAnyValue(property)
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler)
                .BindTo(this, property)
                .DisposeWith(_modelDisposable);
        }
        
        public EntryModel EntryModel { get; }
        
        public void Dispose()
        {
            _modelDisposable.Dispose();
        }
    }
}