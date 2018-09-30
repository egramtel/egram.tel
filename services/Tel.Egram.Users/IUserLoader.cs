using System;

namespace Tel.Egram.Users
{
    public interface IUserLoader
    {
        IObservable<User> GetMe();
    }
}