using System;
using Egram.Components.Authorization;
using Egram.Components.Main;
using Egram.Components.Navigation;
using Egram.Components.Authorization;
using Egram.Components.Navigation;
using ReactiveUI;

namespace Egram.Components.App
{
    public class ApplicationContext : ReactiveObject, IDisposable
    {
        private readonly Authorizer _authorizer;
        private readonly Navigator _navigator;
        
        private readonly MainContextFactory _mainContextFactory;
        private readonly AuthorizationContextFactory _authorizationContextFactory;

        private readonly IDisposable _authorizationStateSubscription;
        private readonly IDisposable _boardNavigationSubscription;

        public ApplicationContext(
            Authorizer authorizer,
            Navigator navigator,
            MainContextFactory mainContextFactory,
            AuthorizationContextFactory authorizationContextFactory
            )
        {
            _authorizer = authorizer;
            _navigator = navigator;
            _mainContextFactory = mainContextFactory;
            _authorizationContextFactory = authorizationContextFactory;

            _authorizationStateSubscription = _authorizer
                .States()
                .Subscribe(ObserveAuthorizationNavigation);
            
            _boardNavigationSubscription = _navigator
                .BoardNavigations()
                .Subscribe(ObserveBoardNavigation);
        }
        
        private AuthorizationContext _authorizationContext;
        public AuthorizationContext AuthorizationContext
        {
            get => _authorizationContext;
            set => this.RaiseAndSetIfChanged(ref _authorizationContext, value);
        }

        private MainContext _mainContext;
        public MainContext MainContext
        {
            get => _mainContext;
            set => this.RaiseAndSetIfChanged(ref _mainContext, value);
        }

        private int _selectedBoardBoardIndex;
        public int SelectedBoardIndex
        {
            get => _selectedBoardBoardIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedBoardBoardIndex, value);
        }

        private async void ObserveAuthorizationNavigation(TD.AuthorizationState state)
        {
            switch (state)
            {
                case TD.AuthorizationState.AuthorizationStateWaitTdlibParameters _:
                    _navigator.Go(Board.None);
                    
                    await _authorizer.SetupParameters();
                    break;
                    
                case TD.AuthorizationState.AuthorizationStateWaitEncryptionKey _:
                    _navigator.Go(Board.None);
                    
                    await _authorizer.CheckEncryptionKey();
                    break;
                    
                case TD.AuthorizationState.AuthorizationStateWaitPhoneNumber _:
                    AuthorizationContext?.Dispose();
                    AuthorizationContext = _authorizationContextFactory.Create();
                    
                    _navigator.Go(Board.Auth);
                    
                    MainContext?.Dispose();
                    MainContext = null;
                    break;
                    
                case TD.AuthorizationState.AuthorizationStateWaitCode _:
                case TD.AuthorizationState.AuthorizationStateWaitPassword _:
                    _navigator.Go(Board.Auth);
                    break;
                
                case TD.AuthorizationState.AuthorizationStateReady _:
                    MainContext?.Dispose();
                    MainContext = _mainContextFactory.Create();
                    
                    _navigator.Go(Board.Main);
                    
                    AuthorizationContext?.Dispose();
                    AuthorizationContext = null;
                    break;
            }
        }

        private void ObserveBoardNavigation(Board board)
        {
            switch (board)
            {
                case Board.None:
                    SelectedBoardIndex = 0;
                    break;
                    
                case Board.Auth:
                    SelectedBoardIndex = 1;
                    break;
                    
                case Board.Main:
                    SelectedBoardIndex = 2;
                    break;
            }
        }

        public void Dispose()
        {
            _authorizationStateSubscription?.Dispose();
            _boardNavigationSubscription?.Dispose();
            
            _authorizationContext?.Dispose();
            _mainContext?.Dispose();
        }
    }
}