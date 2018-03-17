using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleGraphics.Core;
using ConsoleGraphics.Display;
using ConsoleGraphics.ViewConverter;

namespace ConsoleGraphics.Render
{
    public class Renderer : IRender
    {
        private readonly Screen _screen;
        private readonly IRasterizationHandler _rasterizationHandler;
        private readonly ICoordinateConverter _worldToViewBoxConverter;
        private readonly ILuminosityCalculator _luminosityCalculator;

        public Renderer(Screen screen, IRasterizationHandler rasterizationHandler,
            ICoordinateConverter worldToViewBoxConverter,
            ILuminosityCalculator luminosityCalculator)
        {
            _screen = screen;
            _rasterizationHandler = rasterizationHandler;
            _worldToViewBoxConverter = worldToViewBoxConverter;
            _luminosityCalculator = luminosityCalculator;
        }

        public Frame RenderFrame(Scene scene, Camera camera)
        {
            var triangles = scene.Meshes.SelectMany(x => x.Faces);
            triangles = _worldToViewBoxConverter.ConvertListCoordinates(triangles) as IEnumerable<Triangle3D>;

            if (triangles == null)
                throw new FormatException("Cannot convert Mesh faces to Triangle3D list");

            var flatTriangles = triangles.Select(x => (Triangle2D) x);
            return _rasterizationHandler.Rasterize(_screen, flatTriangles);
        }
    }

    public interface IRender 
    {
        Frame RenderFrame(Scene scene, Camera camera);
    }
}
