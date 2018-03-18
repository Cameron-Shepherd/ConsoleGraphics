using System.Collections.Generic;
using ConsoleGraphics.Core;

namespace ConsoleGraphics.Render
{
    public interface IRayGenerator
    {
        IEnumerable<Ray> GenerateNextRays(Ray currentRay, RayIntersection intersection);
    }
}