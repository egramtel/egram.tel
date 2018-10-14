using DynamicData;

namespace Tel.Egram.Components.Messenger.Explorer
{
    public interface IExplorerProvider
    {
        IObservableList<ItemModel> Items { get; }
    }
}