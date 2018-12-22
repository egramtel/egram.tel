using System;

namespace Tel.Egram.Messaging.Chats
{
    public interface IChatLoader
    {
        IObservable<Chat> LoadChat(long chatId);
        IObservable<Chat> LoadChats();
        IObservable<Chat> LoadChannels();
        IObservable<Chat> LoadDirects();
        IObservable<Chat> LoadGroups();
        IObservable<Chat> LoadBots();
        IObservable<Chat> LoadPromo();
    }
}