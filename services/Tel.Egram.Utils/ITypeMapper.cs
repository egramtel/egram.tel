using System;

namespace Tel.Egram.Utils
{
    public interface ITypeMapper
    {
        Type this[Type type] { get; }
    }
}