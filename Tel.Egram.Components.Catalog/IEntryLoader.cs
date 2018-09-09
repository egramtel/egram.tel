using System;
using System.Collections.Generic;

namespace Tel.Egram.Components.Catalog
{
    public interface IEntryLoader
    {
        IObservable<IList<EntryModel>> LoadHomeEntries();
        IObservable<IList<EntryModel>> LoadDirectEntries();
        IObservable<IList<EntryModel>> LoadGroupEntries();
        IObservable<IList<EntryModel>> LoadChannelEntries();
        IObservable<IList<EntryModel>> LoadBotEntries();
    }
}