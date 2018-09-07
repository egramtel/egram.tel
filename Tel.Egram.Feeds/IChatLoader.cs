using System;
using TdLib;

namespace Tel.Egram.Feeds
{
    public interface IChatLoader
    {
        IObservable<Chat> LoadAllChats();
        IObservable<Chat> LoadChannels();
        IObservable<Chat> LoadUsers();
        IObservable<Chat> LoadGroups();
        IObservable<Chat> LoadBots();
    }
}