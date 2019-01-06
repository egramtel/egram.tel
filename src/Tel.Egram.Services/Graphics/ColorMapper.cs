using System;

namespace Tel.Egram.Services.Graphics
{
    public class ColorMapper : IColorMapper
    {
        private readonly string[] _colors =
        {
            "5caae9",
            "e66b66",
            "69cfbe",
            "c57fe1",
            "8ace7c",
            "f5b870"
        };
        
        public ColorMapper()
        {
        }

        public ColorMapper(string[] colors)
        {
            _colors = colors;
        }

        public string this[long id]
        {
            get
            {
                var index = Math.Abs(id) % _colors.Length;
                var color = _colors[index];
                return color;
            }
        }
    }
}