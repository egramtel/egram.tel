using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Splat;
using Tel.Egram.Graphics.Previews;

namespace Tel.Egram.Components.Messenger.Explorer.Messages
{
    public static class PreviewLoadingLogic
    {
        public static IDisposable BindPreviewLoading(
            this PhotoMessageModel model)
        {
            return BindPreviewLoading(
                model,
                Locator.Current.GetService<IPreviewLoader>());
        }
        
        public static IDisposable BindPreviewLoading(
            this PhotoMessageModel model,
            IPreviewLoader previewLoader)
        {
            if (model.Preview == null)
            {
                model.Preview = GetPreview(previewLoader, model);

                if (model.Preview == null || model.Preview.Bitmap == null)
                {
                    return LoadPreview(previewLoader, model)
                        .Subscribe(preview =>
                        {
                            model.Preview = preview;
                        });
                }
            }
            
            return Disposable.Empty;
        }

        private static Preview GetPreview(IPreviewLoader previewLoader, PhotoMessageModel model)
        {
            if (model.Photo != null)
            {
                return previewLoader.GetPreview(model.Photo, PreviewQuality.High);
            }
            
            return null;
        }
        
        private static IObservable<Preview> LoadPreview(IPreviewLoader previewLoader, PhotoMessageModel model)
        {
            if (model.Photo != null)
            {
                return previewLoader.LoadPreview(model.Photo, PreviewQuality.Low)
                    .Concat(previewLoader.LoadPreview(model.Photo, PreviewQuality.High));
            }
            
            return Observable.Empty<Preview>();
        }
        
        
    }
}