using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using ReactiveUI;
using Splat;
using TdLib;
using Tel.Egram.Model.Authentication.Phone;
using Tel.Egram.Model.Authentication.Results;
using Tel.Egram.Services.Authentication;
using Tel.Egram.Services.Utils.Reactive;

namespace Tel.Egram.Model.Authentication
{
    public class AuthenticationManager
    {
        private readonly IAuthenticator _authenticator;

        public AuthenticationManager(
            IAuthenticator authenticator)
        {
            _authenticator = authenticator;
        }

        public AuthenticationManager()
            : this (
                Locator.Current.GetService<IAuthenticator>())
        {
        }
        
        public IDisposable Bind(AuthenticationModel model)
        {
            var canSendCode = model
                .WhenAnyValue(x => x.PhoneNumber)
                .Select(phone => IsPhoneValid(model, phone));
            
            var canCheckCode = model
                .WhenAnyValue(x => x.ConfirmCode)
                .Select(code => !string.IsNullOrWhiteSpace(code));
            
            var canCheckPassword = model
                .WhenAnyValue(x => x.Password)
                .Select(password => !string.IsNullOrWhiteSpace(password));

            var disposable = new CompositeDisposable();

            model.WhenAnyValue(m => m.PhoneNumber)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Accept(phone => HandlePhoneChange(model, phone))
                .DisposeWith(disposable);

            model.WhenAnyValue(m => m.PhoneCode)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Accept(phoneCode => HandlePhoneCodeChange(model, phoneCode))
                .DisposeWith(disposable);
            
            model.SendCodeCommand = ReactiveCommand.CreateFromObservable(
                    (AuthenticationModel m) =>
                    {
                        var phone = m.PhoneCode.Code
                                    + new string(m.PhoneNumber.Where(char.IsDigit).ToArray());
                        return SendCode(phone);
                    },
                    canSendCode,
                    RxApp.MainThreadScheduler)
                .DisposeWith(disposable);

            model.CheckCodeCommand = ReactiveCommand.CreateFromObservable(
                    (AuthenticationModel m) => CheckCode(m.ConfirmCode, m.FirstName, m.LastName),
                    canCheckCode,
                    RxApp.MainThreadScheduler)
                .DisposeWith(disposable);
            
            model.CheckPasswordCommand = ReactiveCommand.CreateFromObservable(
                    (AuthenticationModel m) => CheckPassword(m.Password),
                    canCheckPassword,
                    RxApp.MainThreadScheduler)
                .DisposeWith(disposable);

            _authenticator.ObserveState()
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Accept(state => HandleState(model, state))
                .DisposeWith(disposable);

            return disposable;
        }

        private void HandlePhoneChange(AuthenticationModel model, string phone)
        {
            // TODO: does not update
            model.PhoneNumber = FormatPhone(phone, model.PhoneCode?.Mask);
        }

        private void HandlePhoneCodeChange(AuthenticationModel model, PhoneCodeModel phoneCode)
        {
            model.PhoneNumber = FormatPhone(model.PhoneNumber, phoneCode?.Mask);
        }

        private void HandleState(AuthenticationModel model, TdApi.AuthorizationState state)
        {
            switch (state)
            {
                case TdApi.AuthorizationState.AuthorizationStateWaitPhoneNumber _:
                    OnWaitingPhoneNumber(model);
                    break;
                        
                case TdApi.AuthorizationState.AuthorizationStateWaitCode wait:
                    OnWaitingConfirmCode(model, !wait.IsRegistered);
                    break;
                        
                case TdApi.AuthorizationState.AuthorizationStateWaitPassword _:
                    OnWaitingPassword(model);
                    break;
            }
        }

        private void OnWaitingPhoneNumber(AuthenticationModel model)
        {
            model.ConfirmIndex = 0;
            model.PasswordIndex = 0;
        }

        private void OnWaitingConfirmCode(AuthenticationModel model, bool isRegistration)
        {
            model.IsRegistration = isRegistration;
            
            model.ConfirmIndex = 1;
            model.PasswordIndex = 0;
        }

        private void OnWaitingPassword(AuthenticationModel model)
        {
            model.ConfirmIndex = 1;
            model.PasswordIndex = 1;
        }

        private IObservable<SendCodeResult> SendCode(string phoneNumber)
        {   
            return _authenticator
                .SetPhoneNumber(phoneNumber)
                .Select(_ => new SendCodeResult());
        }

        private IObservable<CheckCodeResult> CheckCode(
            string code,
            string firstName,
            string lastName)
        {
            return _authenticator
                .CheckCode(code, firstName, lastName)
                .Select(_ => new CheckCodeResult());
        }

        private IObservable<CheckPasswordResult> CheckPassword(string password)
        {
            return _authenticator
                .CheckPassword(password)
                .Select(_ => new CheckPasswordResult());
        }

        private bool IsPhoneValid(AuthenticationModel model, string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return false;
            }
            
            var mask = model.PhoneCode?.Mask;
            
            if (string.IsNullOrWhiteSpace(mask))
            {
                return !string.IsNullOrWhiteSpace(phone);
            }

            return phone.All(c => char.IsDigit(c) || char.IsWhiteSpace(c))
                && phone.Count(char.IsDigit) == mask.Count(c => !char.IsWhiteSpace(c));
        }

        private string FormatPhone(string phone, string mask)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return "";
            }

            var numbers = new string(phone.Where(char.IsDigit).ToArray());
            
            if (string.IsNullOrWhiteSpace(mask))
            {
                return numbers;
            }
            
            var builder = new StringBuilder(mask.Length);

            int i = 0;
            foreach (var ch in mask)
            {
                if (i < numbers.Length)
                {
                    if (char.IsWhiteSpace(ch))
                    {
                        builder.Append(" ");
                    }
                    else
                    {
                        builder.Append(numbers[i]);
                        i++;
                    }
                }
            }

            return builder.ToString();
        }
    }
}