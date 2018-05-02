using System;

namespace Egram.Components.Graphics
{
    public class ColorMaker
    {
        public string GetHexFromId(long id)
        {
            var index = Math.Abs(id) % _colors.Length;
            var color = _colors[index];
            return color;
        }
        
        private readonly string[] _colors =
        {
            "5caae9",
            "e66b66",
            "69cfbe",
            "c57fe1",
            "8ace7c",
            "f5b870"
        };
    }
}