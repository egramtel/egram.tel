using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Egram.Components.Graphics;
using Egram.Components.TDLib;

namespace Egram.Components.Chatting
{
    public class UserAuthorLoader : IDisposable
    {
        private readonly IAgent _agent;
        private readonly AvatarLoader _avatarLoader;
        private readonly ConcurrentDictionary<int, UserAuthor> _userAuthorCache;

        public UserAuthorLoader(
            IAgent agent,
            AvatarLoader avatarLoader
            )
        {
            _agent = agent;
            _avatarLoader = avatarLoader;
            _userAuthorCache = new ConcurrentDictionary<int, UserAuthor>();
        }

        public bool Retrieve(int userId, out UserAuthor userAuthor)
        {
            if (_userAuthorCache.TryGetValue(userId, out userAuthor))
            {
                return true;
            }
            
            userAuthor = new UserAuthor
            {
                UserId = userId
            };

            if (_userAuthorCache.TryAdd(userId, userAuthor))
            {
                return false;
            }

            _userAuthorCache.TryGetValue(userId, out userAuthor);
            
            return true;
        }

        public IObservable<Load> LoadAll(IList<int> userIds)
        {
            return Observable.Create<Load>(async observer =>
            {
                var users = new List<TD.User>(userIds.Count);
                
                // load info
                foreach (var userId in userIds)
                {
                    if (_userAuthorCache.TryGetValue(userId, out var userAuthor))
                    {
                        var user = await _agent.ExecuteAsync(
                            new TD.GetUser
                            {
                                UserId = userId
                            });
                    
                        users.Add(user);
                    
                        observer.OnNext(new Load(
                            userAuthor,
                            user.FirstName + (user.LastName != null ? " " + user.LastName : "")
                        ));
                    }
                }

                // load avatars
                foreach (var user in users)
                {
                    if (_userAuthorCache.TryGetValue(user.Id, out var userAuthor))
                    {
                        var bitmap = await _avatarLoader.LoadForUserAsync(user);
                    
                        observer.OnNext(new Load(
                            userAuthor,
                            bitmap
                        ));
                    }
                }

                observer.OnCompleted();
            });
        }

        public void Dispose()
        {
            
        }
        
        public class Load
        {
            public readonly UserAuthor UserAuthor;
            public readonly IBitmap Avatar;
            public readonly string Name;

            public Load(UserAuthor userAuthor, string name)
            {
                UserAuthor = userAuthor;
                Name = name;
            }

            public Load(UserAuthor userAuthor, IBitmap avatar)
            {
                UserAuthor = userAuthor;
                Avatar = avatar;
            }
        }
    }
}