namespace Tel.Egram.Utils
{
    public struct Range
    {
        public readonly int From;
        public readonly int To;

        public Range(int from, int to)
        {
            From = from;
            To = to;
        }
        
        public override bool Equals(object obj) 
        {
            return obj is Range range && this == range;
        }

        public override int GetHashCode() 
        {
            return From ^ To;
        }
        
        public static bool operator ==(Range x, Range y) 
        {
            return x.From == y.From && x.To == y.To;
        }
        
        public static bool operator !=(Range x, Range y) 
        {
            return !(x == y);
        }
        
        public override string ToString()
        {
            return $"({From}, {To})";
        }
    }
}