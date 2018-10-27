using System;
using Tel.Egram.Gui.Views.Messenger.Catalog;

namespace Tel.Egram.Components.Messenger.Catalog
{
    public interface ICatalogController : IDisposable
    {
        CatalogControlModel Model { get; }
    }
}