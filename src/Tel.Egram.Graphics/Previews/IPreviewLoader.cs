using System;
using TdLib;

namespace Tel.Egram.Graphics.Previews
{
    public interface IPreviewLoader
    {
        Preview GetPreview(TdApi.Photo photo, PreviewQuality quality);

        IObservable<Preview> LoadPreview(TdApi.Photo photo, PreviewQuality quality);
    }
}