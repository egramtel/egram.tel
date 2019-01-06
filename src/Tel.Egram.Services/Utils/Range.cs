namespace Tel.Egram.Services.Utils
{
    public struct Range
    {
        public int Index { get; }
        public int Length { get; }
        public int LastIndex => Index + Length - 1;

        public Range(int index, int length)
        {
            Index = index;
            Length = length;
        }
        
        public override bool Equals(object obj)
        {
            return obj is Range range && this == range;
        }

        public override int GetHashCode()
        {
            return Index ^ Length;
        }
        
        public static bool operator ==(Range x, Range y)
        {
            return x.Index == y.Index && x.Length == y.Length;
        }
        
        public static bool operator !=(Range x, Range y)
        {
            return !(x == y);
        }
        
        public override string ToString()
        {
            return Length == 0 ? $"({Length})" : $"({Length}, {Index}-{LastIndex})";
        }
    }
}