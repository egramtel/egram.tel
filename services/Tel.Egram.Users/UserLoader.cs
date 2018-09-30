using System;
using System.Reactive.Linq;
using TdLib;
using Tel.Egram.TdLib;

namespace Tel.Egram.Users
{
    public class UserLoader : IUserLoader
    {
        private readonly TdAgent _agent;

        public UserLoader(
            TdAgent agent
            )
        {
            _agent = agent;
        }
        
        public IObservable<User> GetMe()
        {
            return _agent.Execute(new TdApi.GetMe())
                .Select(user => new User
                {
                    UserData = user
                });
        }
    }
}