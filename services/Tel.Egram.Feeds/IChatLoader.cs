using System;

namespace Tel.Egram.Feeds
{
    public interface IChatLoader
    {
        IObservable<Chat> LoadChats();
        IObservable<Chat> LoadChannels();
        IObservable<Chat> LoadDirects();
        IObservable<Chat> LoadGroups();
        IObservable<Chat> LoadBots();
    }
}