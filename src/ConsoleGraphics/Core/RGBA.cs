using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGraphics.Core
{
    public partial class Rgba
    {
        public float Red { get; }
        public float Green { get; }
        public float Blue { get; }
        public float Alpha { get; }

        public Rgba(float red, float green, float blue, float alpha)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }

        public static Rgba operator *(Rgba colour, float intensity)
        {
            return new Rgba(colour.Red * intensity, colour.Green * intensity, colour.Blue * intensity, colour.Alpha);
        }

        public static Rgba operator +(Rgba colour, Rgba colour2)
        {
            return new Rgba(colour.Red + colour2.Red, colour.Green * colour2.Green, colour.Blue * colour2.Blue, colour.Alpha);
        }
    }

    public partial class Rgba
    {
        public static readonly Rgba NoLight = new Rgba(0, 0, 0, 0);
    }
}
