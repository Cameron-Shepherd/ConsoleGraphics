using System.Collections.Generic;
using ConsoleGraphics.Core;
using ConsoleGraphics.Core.Light;

namespace ConsoleGraphics.Render
{
    public interface ILuminosityCalculator
    {
        byte Luminosity(Point3D location, Vector normal, Light sourceLight);
        byte Luminosity(Point3D location, Vector normal, IEnumerable<Light> sourceLight);
    }
}
