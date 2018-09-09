using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Tel.Egram.Feeds;

namespace Tel.Egram.Components.Catalog
{
    public class EntryLoader : IEntryLoader
    {
        private readonly IChatLoader _chatLoader;

        public EntryLoader(IChatLoader chatLoader)
        {
            _chatLoader = chatLoader;
        }
        
        public IObservable<IList<EntryModel>> LoadHomeEntries()
        {
            return LoadEntries(_chatLoader.LoadChannels)
                .Select(list =>
                {
                    var aggregateEntry = AggregateEntryModel.Main();
                    list.Insert(0, aggregateEntry);
                    return list;
                });
        }

        public IObservable<IList<EntryModel>> LoadDirectEntries()
        {
            return LoadEntries(_chatLoader.LoadDirects);
        }

        public IObservable<IList<EntryModel>> LoadGroupEntries()
        {
            return LoadEntries(_chatLoader.LoadGroups);
        }

        public IObservable<IList<EntryModel>> LoadBotEntries()
        {
            return LoadEntries(_chatLoader.LoadBots);
        }

        private IObservable<IList<EntryModel>> LoadEntries(Func<IObservable<Chat>> loader)
        {
            return loader()
                .Select(ChatEntryModel.FromChat)
                .Aggregate(new List<ChatEntryModel>(), (list, chat) =>
                {
                    list.Add(chat);
                    return list;
                })
                .Select(list => list.OrderBy(model => !model.Chat.Ch.IsPinned)
                    .ThenByDescending(model => model.Chat.Ch.LastMessage.Date)
                    .Cast<EntryModel>()
                    .ToList());
        }
    }
}