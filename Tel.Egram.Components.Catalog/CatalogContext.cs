using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Media;
using Avalonia.Threading;
using PropertyChanged;
using ReactiveUI;
using TdLib;
using Tel.Egram.Graphics;

namespace Tel.Egram.Components.Catalog
{
    [AddINotifyPropertyChangedInterface]
    public class CatalogContext : IDisposable
    {
        private readonly CompositeDisposable _contextDisposable = new CompositeDisposable();
        private readonly IAvatarLoader _avatarLoader;
        private readonly IEntryLoader _entryLoader;
        private readonly IColorMapper _colorMapper;
        private readonly CatalogKind _kind;
        
        public ReactiveList<EntryModel> Entries { get; set; } = new ReactiveList<EntryModel>();
        public int SelectedEntryIndex { get; set; } = -1;

        public CatalogContext(
            IAvatarLoader avatarLoader,
            IEntryLoader entryLoader,
            IColorMapper colorMapper,
            CatalogKind kind)
        {
            _entryLoader = entryLoader;
            _avatarLoader = avatarLoader;
            _colorMapper = colorMapper;
            _kind = kind;

            IObservable<IList<EntryModel>> entryLoaderObservable;
            
            switch (_kind)
            {
                case CatalogKind.Bots:
                    entryLoaderObservable = _entryLoader.LoadBotEntries();
                    break;
                
                case CatalogKind.Channels:
                    entryLoaderObservable = _entryLoader.LoadChannelEntries();
                    break;
                
                case CatalogKind.Groups:
                    entryLoaderObservable = _entryLoader.LoadGroupEntries();
                    break;
                
                case CatalogKind.Direct:
                    entryLoaderObservable = _entryLoader.LoadDirectEntries();
                    break;
                
                case CatalogKind.Home:
                default:
                    entryLoaderObservable = _entryLoader.LoadHomeEntries();
                    break;
            }

            entryLoaderObservable
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(AvaloniaScheduler.Instance)
                .Subscribe(HandleEntriesLoaded)
                .DisposeWith(_contextDisposable);
        }

        private void HandleEntriesLoaded(IList<EntryModel> entries)
        {
            foreach (var entry in entries)
            {
                LoadAvatar(entry).DisposeWith(_contextDisposable);
            }

            Entries = new ReactiveList<EntryModel>(entries);
            SelectedEntryIndex = 0;
        }

        private IDisposable LoadAvatar(EntryModel entry)
        {
            switch (entry)
            {
                case ChatEntryModel chatEntry:
                    return _avatarLoader.LoadAvatar(chatEntry.Chat.ChatData, AvatarSize.Small)
                        .SubscribeOn(TaskPoolScheduler.Default)
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .Subscribe(avatar =>
                        {
                            chatEntry.Avatar = avatar;
                        });
                
                case AggregateEntryModel aggregateEntry:
                    return _avatarLoader.LoadAvatar(new TdApi.Chat
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

        public void Dispose()
        {
            _contextDisposable.Dispose();
        }
    }
}