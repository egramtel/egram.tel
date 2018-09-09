using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Tel.Egram.Graphics;
using Tel.Egram.Users;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Navigation
{
    public class NavigationContext : ReactiveObject, IDisposable
    {
        private readonly CompositeDisposable _contextDisposable = new CompositeDisposable();
        
        private readonly IUserLoader _userLoader;
        private readonly IAvatarLoader _avatarLoader;
        
        public NavigationContext(
            IUserLoader userLoader,
            IAvatarLoader avatarLoader
            )
        {
            _userLoader = userLoader;
            _avatarLoader = avatarLoader;
            
            _userLoader.GetMe()
                .SelectMany(user => _avatarLoader.LoadBitmap(user.UserData, AvatarSize.Big))
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(AvaloniaScheduler.Instance)
                .Subscribe(HandleProfilePhoto)
                .DisposeWith(_contextDisposable);
        }
        
        private IBitmap _profilePhoto;
        public IBitmap ProfilePhoto
        {
            get => _profilePhoto;
            set => this.RaiseAndSetIfChanged(ref _profilePhoto, value);
        }

        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedTabIndex, value);
        }

        public void HandleProfilePhoto(IBitmap bitmap)
        {
            ProfilePhoto = bitmap;
        }
        
        public void Dispose()
        {
            _contextDisposable.Dispose();
        }
    }
}