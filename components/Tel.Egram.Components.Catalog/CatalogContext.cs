using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Media;
using Avalonia.Threading;
using DynamicData;
using DynamicData.Binding;
using PropertyChanged;
using ReactiveUI;
using TdLib;
using Tel.Egram.Graphics;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Catalog
{
    [AddINotifyPropertyChangedInterface]
    public class CatalogContext : IDisposable
    {
        private readonly CompositeDisposable _contextDisposable = new CompositeDisposable();
        
        public EntryModel SelectedEntry { get; set; }
        
        public ObservableCollectionExtended<EntryModel> Entries { get; }
            = new ObservableCollectionExtended<EntryModel>();

        public CatalogContext(
            IFactory<ICatalogProvider> catalogProviderFactory,
            IAvatarLoader avatarLoader,
            CatalogKind kind)
        {
            var catalogService = catalogProviderFactory.Create();
            
            BindCatalog(catalogService, kind).DisposeWith(_contextDisposable);
            BindAvatarLoader(avatarLoader).DisposeWith(_contextDisposable);
        }

        private IDisposable BindCatalog(ICatalogProvider catalogProvider, CatalogKind kind)
        {
            IObservable<IChangeSet<EntryModel, long>> changes;
            
            switch (kind)
            {
                case CatalogKind.Bots:
                    changes = catalogProvider.Chats.Connect()
                        .Filter(catalogProvider.BotFilter);
                    break;
                
                case CatalogKind.Channels:
                    changes = catalogProvider.Chats.Connect()
                        .Filter(catalogProvider.ChannelFilter);
                    break;
                
                case CatalogKind.Groups:
                    changes = catalogProvider.Chats.Connect()
                        .Filter(catalogProvider.GroupFilter);
                    break;
                
                case CatalogKind.Direct:
                    changes = catalogProvider.Chats.Connect()
                        .Filter(catalogProvider.DirectFilter);
                    break;
                
                case CatalogKind.Home:
                default:
                    changes = catalogProvider.Chats.Connect();
                    break;
            }
            
            return changes
                .Sort(GetSorting())
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(Entries)
                .Subscribe();
        }

        private IDisposable BindAvatarLoader(IAvatarLoader avatarLoader)
        {
            return Entries.ObserveCollectionChanges()
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Select(e => e.EventArgs)
                .Subscribe(args =>
                {
                    var entries = Entries;
                    
                    switch (args.Action)
                    {
                        case NotifyCollectionChangedAction.Reset:
                            foreach (var entry in entries)
                            {
                                if (entry.Avatar is null)
                                {
                                    LoadAvatar(avatarLoader, entry)
                                        .DisposeWith(_contextDisposable);
                                }
                            }
                            break;
                        
                        case NotifyCollectionChangedAction.Add:
                        case NotifyCollectionChangedAction.Replace:
                            foreach (var item in args.NewItems)
                            {
                                if (item is EntryModel entry && entry.Avatar is null)
                                {
                                    LoadAvatar(avatarLoader, entry)
                                        .DisposeWith(_contextDisposable);
                                }
                            }
                            break;
                    }
                });
        }

        private IDisposable LoadAvatar(IAvatarLoader avatarLoader, EntryModel entry)
        {
            switch (entry)
            {
                case ChatEntryModel chatEntry:
                    return avatarLoader.LoadAvatar(chatEntry.Chat.ChatData, AvatarSize.Small)
                        .SubscribeOn(TaskPoolScheduler.Default)
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .Subscribe(avatar =>
                        {
                            chatEntry.Avatar = avatar;
                        });
                
                case AggregateEntryModel aggregateEntry:
                    return avatarLoader.LoadAvatar(new TdApi.Chat
                        {
                            Id = aggregateEntry.Aggregate.Id
                        }, AvatarSize.Small)
                        .SubscribeOn(TaskPoolScheduler.Default)
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .Subscribe(avatar =>
                        {
                            aggregateEntry.Avatar = avatar;
                        });
            }
            
            return Disposable.Empty;
        }

        private IComparer<EntryModel> GetSorting()
        {
            return SortExpressionComparer<EntryModel>.Ascending(p => p.Order);
        }

        public void Dispose()
        {
            _contextDisposable.Dispose();
        }
    }
}