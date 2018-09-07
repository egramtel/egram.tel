using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Avalonia.Threading;
using TdLib;
using Tel.Egram.Graphics;
using Tel.Egram.TdLib;

namespace Tel.Egram.Components.Navigation
{
    public class ProfileInteractor
    {
        private readonly TdAgent _agent;
        private readonly IAvatarLoader _avatarLoader;

        public ProfileInteractor(
            TdAgent agent,
            IAvatarLoader avatarLoader
            )
        {
            _agent = agent;
            _avatarLoader = avatarLoader;
        }

        public IDisposable LoadAvatar(NavigationContext context)
        {
            return _agent.Execute(new TdApi.GetMe())
                .SelectMany(user => _avatarLoader.LoadBitmap(user, AvatarSize.Big))
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(AvaloniaScheduler.Instance)
                .Subscribe(bitmap =>
                {
                    context.OnProfilePhotoLoaded(bitmap);
                });
        }
    }
}