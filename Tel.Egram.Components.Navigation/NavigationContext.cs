using System;
using Avalonia.Media.Imaging;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Navigation
{
    public class NavigationContext : ReactiveObject, IDisposable
    {
        private readonly ProfileInteractor _profileInteractor;
        private readonly IDisposable _avatarLoadSubscription;
        
        public NavigationContext(
            IFactory<ProfileInteractor> profileInteractorFactory
            )
        {
            _profileInteractor = profileInteractorFactory.Create();
            _avatarLoadSubscription = _profileInteractor.LoadAvatar(this);
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

        public void OnProfilePhotoLoaded(IBitmap bitmap)
        {
            ProfilePhoto = bitmap;
        }
        
        public void Dispose()
        {
            _avatarLoadSubscription.Dispose();
        }
    }
}