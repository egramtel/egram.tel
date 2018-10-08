using System;

namespace Tel.Egram.Messaging.Users
{
    public interface IUserLoader
    {
        IObservable<User> GetMe();
    }
}