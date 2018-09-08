using System;
using Avalonia.Media.Imaging;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
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
        
        [Reactive]
        public IBitmap ProfilePhoto { get; set; }

        [Reactive]
        public int? SelectedTabIndex { get; set; }

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