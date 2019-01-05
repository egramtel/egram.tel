using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using Splat;
using Tel.Egram.Components.Popups;
using Tel.Egram.Utils.Reactive;

namespace Tel.Egram.Components.Application
{
    public static class PopupsLogic
    {
        public static IDisposable BindPopup(
            this MainWindowModel model)
        {
            return BindPopup(
                model,
                Locator.Current.GetService<IPopupController>());
        }
        
        public static IDisposable BindPopup(
            this MainWindowModel model,
            IPopupController popupController)
        {
            model.PopupModel = PopupModel.Hidden();
            
            var trigger = (popupController as PopupController)?.Trigger;

            if (trigger != null)
            {
                return trigger
                    .SubscribeOn(RxApp.TaskpoolScheduler)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Accept(context =>
                    {
                        model.PopupModel = context == null
                            ? PopupModel.Hidden()
                            : new PopupModel(context);
                    });
            }
            
            return Disposable.Empty;
        }
    }
}