using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using PropertyChanged;
using Tel.Egram.Graphics;
using Tel.Egram.Users;

namespace Tel.Egram.Components.Navigation
{
    [AddINotifyPropertyChangedInterface]
    public class NavigationContext : IDisposable
    {
        private readonly CompositeDisposable _contextDisposable = new CompositeDisposable();
        private readonly IAvatarLoader _avatarLoader;
        private readonly IUserLoader _userLoader;
        
        public IBitmap ProfilePhoto { get; set; }
        public int SelectedTabIndex { get; set; }
        
        public NavigationContext(
            IAvatarLoader avatarLoader,
            IUserLoader userLoader)
        {
            _avatarLoader = avatarLoader;
            _userLoader = userLoader;
            _userLoader
                .GetMe()
                .SelectMany(user => _avatarLoader.LoadBitmap(user.UserData, AvatarSize.Big))
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(AvaloniaScheduler.Instance)
                .Subscribe(HandleProfilePhoto)
                .DisposeWith(_contextDisposable);
        }

        public void HandleProfilePhoto(IBitmap bitmap) => ProfilePhoto = bitmap;

        public void Dispose() => _contextDisposable.Dispose();
    }
}