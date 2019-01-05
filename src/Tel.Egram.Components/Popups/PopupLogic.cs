using System;
using System.Reactive.Linq;
using ReactiveUI;
using Splat;
using Tel.Egram.Utils.Reactive;

namespace Tel.Egram.Components.Popups
{
    public static class PopupLogic
    {
        public static IDisposable BindPopup(
            this PopupModel model)
        {
            return BindPopup(
                model,
                Locator.Current.GetService<IPopupController>());
        }
        
        public static IDisposable BindPopup(
            this PopupModel model,
            IPopupController popupController)
        {
            return model.Context.CloseCommand
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Accept(_ =>
                {
                    popupController.Hide();
                });
        }
    }
}