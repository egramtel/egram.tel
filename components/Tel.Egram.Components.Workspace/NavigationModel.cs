using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Threading;
using PropertyChanged;
using Tel.Egram.Graphics;
using Tel.Egram.Messaging.Users;

namespace Tel.Egram.Components.Workspace
{
    [AddINotifyPropertyChangedInterface]
    public class NavigationModel : IDisposable
    {
        private readonly CompositeDisposable _modelDisposable = new CompositeDisposable();
        
        public Avatar Avatar { get; set; }
        
        public int SelectedTabIndex { get; set; }
        
        public NavigationModel(
            IAvatarLoader avatarLoader,
            IUserLoader userLoader)
        {
            LoadAvatar(userLoader, avatarLoader)
                .DisposeWith(_modelDisposable);
        }

        private IDisposable LoadAvatar(IUserLoader userLoader, IAvatarLoader avatarLoader)
        {
            return userLoader
                .GetMe()
                .SelectMany(user => avatarLoader.LoadAvatar(user.UserData, AvatarSize.Big))
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(AvaloniaScheduler.Instance)
                .Subscribe(avatar =>
                {
                    Avatar = avatar;
                });
        }

        public void Dispose() => _modelDisposable.Dispose();
    }
}