using System;

namespace Tel.Egram.Services.Messaging.Users
{
    public interface IUserLoader
    {
        IObservable<User> GetMe();
    }
}