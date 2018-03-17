using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGraphics.Core
{
    public class Rgba
    {
        public byte Red { get; }
        public byte Green { get; }
        public byte Blue { get; }
        public float Alpha { get; }

        public Rgba(byte red, byte green, byte blue, float alpha)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }
    }
}
