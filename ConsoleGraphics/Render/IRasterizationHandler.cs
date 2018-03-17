using System.Collections.Generic;
using ConsoleGraphics.Core;
using ConsoleGraphics.Display;

namespace ConsoleGraphics.Render
{
    public interface IRasterizationHandler
    {
        Frame Rasterize(Screen screen, IEnumerable<Triangle2D> triangles);
    }
}