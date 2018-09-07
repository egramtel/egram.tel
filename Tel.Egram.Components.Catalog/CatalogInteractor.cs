using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Avalonia.Media;
using Avalonia.Threading;
using ReactiveUI;
using TdLib;
using Tel.Egram.Feeds;
using Tel.Egram.Graphics;

namespace Tel.Egram.Components.Catalog
{
    public class CatalogInteractor
    {
        private readonly IChatLoader _chatLoader;
        private readonly IAvatarLoader _avatarLoader;
        private readonly IColorMapper _colorMapper;

        public CatalogInteractor(
            IChatLoader chatLoader,
            IAvatarLoader avatarLoader,
            IColorMapper colorMapper
            )
        {
            _chatLoader = chatLoader;
            _avatarLoader = avatarLoader;
            _colorMapper = colorMapper;
        }

        public IDisposable LoadCatalog(CatalogContext context, CatalogKind kind)
        {
            return LoadAllChats(kind)
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(AvaloniaScheduler.Instance)
                .Subscribe(section =>
                {
                    context.OnSectionLoaded(new SectionModel
                    {
                        Title = section.Item1.ToString(),
                        Entries = new ReactiveList<EntryModel>(section.Item2.Select(chat =>
                        {
                            var chatModel = ChatEntryModel.FromChat(chat);
                            var color = Color.Parse("#" + _colorMapper[chat.Ch.Id]);
                            chatModel.ColorBrush = new SolidColorBrush(color);
                            chatModel.LoadAvatar = () => LoadAvatar(chatModel);
                            return chatModel;
                        }))
                    });
                });
        }

        private IDisposable LoadAvatar(ChatEntryModel chatEntryModel)
        {
            if (chatEntryModel.Avatar == null)
            {
                chatEntryModel.Avatar = _avatarLoader.GetBitmap(chatEntryModel.Chat.Ch, AvatarSize.Small);
                chatEntryModel.IsFallbackAvatar = chatEntryModel.Avatar == null;
            }
        
            return _avatarLoader.LoadBitmap(chatEntryModel.Chat.Ch, AvatarSize.Small)
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(AvaloniaScheduler.Instance)
                .Subscribe(bitmap =>
                {
                    chatEntryModel.Avatar = bitmap;
                    chatEntryModel.IsFallbackAvatar = chatEntryModel.Avatar == null;
                });
        }

        private IObservable<Tuple<CatalogKind, List<Chat>>> LoadAllChats(CatalogKind kind)
        {
            return LoadChats(kind)
                .Aggregate(new List<Chat>(), (list, chat) =>
                {
                    list.Add(chat);
                    return list;
                })
                .Select(list => list.OrderByDescending(chat => chat.Ch.LastMessage.Date)
                    .ToList())
                .Select(chats => Tuple.Create(kind, chats));
        }

        private IObservable<Chat> LoadChats(CatalogKind kind)
        {
            switch (kind)
            {
                case CatalogKind.Home:
                    return _chatLoader.LoadChannels();

                case CatalogKind.Direct:
                    return _chatLoader.LoadUsers();

                case CatalogKind.Groups:
                    return _chatLoader.LoadGroups();

                case CatalogKind.Bots:
                    return _chatLoader.LoadBots();
            }

            return Observable.Empty<Chat>();
        }
    }
}