using System;

namespace Egram.Components.Navigation
{
    [Flags]
    public enum ExplorerEntityKind
    {
        Header = 1 << 0,
        Bot = 1 << 1,
        Channel = 1 << 2,
        Group = 1 << 3,
        People = 1 << 4
    }
}