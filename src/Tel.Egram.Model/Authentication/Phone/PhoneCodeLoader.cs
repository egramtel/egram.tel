using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using DynamicData.Binding;
using Splat;
using Tel.Egram.Services.Persistance;
using Tel.Egram.Services.Utils.Reactive;
using IBitmap = Avalonia.Media.Imaging.IBitmap;

namespace Tel.Egram.Model.Authentication.Phone
{
    public class PhoneCodeLoader
    {
        private readonly IResourceManager _resourceManager;

        public PhoneCodeLoader(
            IResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        public PhoneCodeLoader()
            : this(
                Locator.Current.GetService<IResourceManager>())
        {
        }
        
        public IDisposable Bind(AuthenticationModel model)
        {
            return Task.Run(() =>
                {
                    var assetLoader = AvaloniaLocator.Current.GetService<IAssetLoader>();
                    var codes = _resourceManager.GetPhoneCodes();
    
                    model.PhoneCodes = new ObservableCollectionExtended<PhoneCodeModel>(
                        codes
                            .Select(c => new PhoneCodeModel
                            {
                                Code = "+" + c.Code,
                                CountryCode = c.CountryCode,
                                Flag = GetFlag(assetLoader, c.CountryCode),
                                Mask = c.Mask?.ToLowerInvariant()
                            })
                            .OrderBy(m => m.CountryCode));

                    model.PhoneCode = model.PhoneCodes.FirstOrDefault(c => c.CountryCode == "RU");
                })
                .ToObservable()
                .Accept();
        }

        private IBitmap GetFlag(IAssetLoader assetLoader, string countryCode)
        {
            var uri = new Uri($"resm:Tel.Egram.Application.Images.Flags.{countryCode}.png?assembly=Tel.Egram.Application");
            if (!assetLoader.Exists(uri))
            {
                uri = new Uri($"resm:Tel.Egram.Application.Images.Flags._unknown.png?assembly=Tel.Egram.Application");
            }

            using (var stream = assetLoader.Open(uri))
            {
                return new Bitmap(stream);
            }
        }
    }
}