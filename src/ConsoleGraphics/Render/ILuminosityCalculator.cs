using System.Collections.Generic;
using ConsoleGraphics.Core;
using ConsoleGraphics.Core.Light;

namespace ConsoleGraphics.Render
{
    public interface ILuminosityCalculator
    {
        Rgba Luminosity(Point4D location, Vector normal, Light sourceLight);
        Rgba Luminosity(Point4D location, Vector normal, IEnumerable<Light> sourceLight);
    }
}
