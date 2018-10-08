using System;
using System.Reactive.Linq;
using TdLib;
using Tel.Egram.Utils.TdLib;

namespace Tel.Egram.Messaging.Users
{
    public class UserLoader : IUserLoader
    {
        private readonly IAgent _agent;

        public UserLoader(
            IAgent agent
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