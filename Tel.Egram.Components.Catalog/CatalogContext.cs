using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Media;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using TdLib;
using Tel.Egram.Feeds;
using Tel.Egram.Graphics;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Catalog
{
    public class CatalogContext : ReactiveObject, IDisposable
    {
        private readonly CatalogKind _kind;
        private readonly IEntryLoader _entryLoader;
        private readonly IAvatarLoader _avatarLoader;
        private readonly IColorMapper _colorMapper;

        private readonly IDisposable _entryLoaderSubscription;

        public CatalogContext(
            CatalogKind kind,
            IEntryLoader entryLoader,
            IAvatarLoader avatarLoader,
            IColorMapper colorMapper
            )
        {
            _kind = kind;
            _entryLoader = entryLoader;
            _avatarLoader = avatarLoader;
            _colorMapper = colorMapper;

            IObservable<IList<EntryModel>> entryLoaderObservable;
            
            switch (kind)
            {
                case CatalogKind.Bots:
                    entryLoaderObservable = _entryLoader.LoadBotEntries();
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

            _entryLoaderSubscription = entryLoaderObservable
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(AvaloniaScheduler.Instance)
                .Subscribe(HandleEntriesLoaded);
        }
        
        private SectionModel _section;
        public SectionModel Section
        {
            get => _section;
            set => this.RaiseAndSetIfChanged(ref _section, value);
        }

        private void HandleEntriesLoaded(IList<EntryModel> entries)
        {
            foreach (var entry in entries)
            {
                LoadAvatar(entry);
            }
            
            Section = new SectionModel
            {
                Entries = new ReactiveList<EntryModel>(entries)
            };
        }

        private IDisposable LoadAvatar(EntryModel entry)
        {
            switch (entry)
            {
                case ChatEntryModel chatEntry:
                    return _avatarLoader.LoadBitmap(chatEntry.Chat.Ch, AvatarSize.Small)
                        .SubscribeOn(TaskPoolScheduler.Default)
                        .ObserveOn(AvaloniaScheduler.Instance)
                        .Subscribe(bitmap =>
                        {
                            var color = Color.Parse("#" + _colorMapper[chatEntry.Chat.Ch.Id]);
                            chatEntry.ColorBrush = new SolidColorBrush(color);
                            
                            chatEntry.Avatar = bitmap;
                            chatEntry.IsFallbackAvatar = chatEntry.Avatar == null;
                        });
                
                case AggregateEntryModel aggregateEntry:
                    return _avatarLoader.LoadBitmap(new TdApi.Chat
                        {
                            Id = aggregateEntry.Aggregate.Id
                        }, AvatarSize.Small)
                        .SubscribeOn(TaskPoolScheduler.Default)
                        .ObserveOn(AvaloniaScheduler.Instance)
                        .Subscribe(bitmap =>
                        {
                            var color = Color.Parse("#" + _colorMapper[aggregateEntry.Aggregate.Id]);
                            aggregateEntry.ColorBrush = new SolidColorBrush(color);
                            
                            aggregateEntry.Avatar = bitmap;
                            aggregateEntry.IsFallbackAvatar = aggregateEntry.Avatar == null;
                        });
            }
            
            return Disposable.Empty;
        }

        public void Dispose()
        {
            _entryLoaderSubscription.Dispose();
        }
    }
}