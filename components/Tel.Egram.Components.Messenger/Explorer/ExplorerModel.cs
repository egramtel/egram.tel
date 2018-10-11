using System;
using System.Reactive.Disposables;
using PropertyChanged;
using Tel.Egram.Messaging.Chats;

namespace Tel.Egram.Components.Messenger.Explorer
{
    [AddINotifyPropertyChangedInterface]
    public class ExplorerModel : IDisposable
    {
        private readonly CompositeDisposable _modelDisposable = new CompositeDisposable();

        public ExplorerModel(Target target)
        {
            
        }

        public void Dispose()
        {
            _modelDisposable.Dispose();
        }
    }
}