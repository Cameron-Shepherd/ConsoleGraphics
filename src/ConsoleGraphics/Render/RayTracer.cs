using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleGraphics.Core;
using ConsoleGraphics.Core.Light;
using ConsoleGraphics.Display;
using ConsoleGraphics.ViewConverter;

namespace ConsoleGraphics.Render
{
    public class RayTracer : IRender
    {
        private readonly Screen _screen;
        private readonly IWorldToCameraCoordinateConverter _worldToCameraCoordinateConverter;
        private readonly ILuminosityCalculator _luminosityCalculator;


        public RayTracer(Screen screen, 
            IWorldToCameraCoordinateConverter worldToCameraCoordinateConverter,
            ILuminosityCalculator luminosityCalculator)
        {
            _screen = screen;
            _worldToCameraCoordinateConverter = worldToCameraCoordinateConverter;
            _luminosityCalculator = luminosityCalculator;
        }

        public Frame RenderFrame(Scene scene, Camera camera)
        {
            //var triangles = scene.Meshes.SelectMany(x => x.Faces);
            //triangles = _worldToCameraCoordinateConverter.ConvertListToCameraView(triangles) as IEnumerable<Triangle3D>;

            //if (triangles == null)
            //    throw new FormatException("Cannot convert Mesh faces to Triangle3D list");
            var pixels = new Pixel[_screen.Width, _screen.Height];
            var opposite = 2 * Math.Tan(camera.ViewAngle);

            for (var i = 0; i < _screen.Width; i++)
            {
                for (var j = 0; j < _screen.Height; j++)
                {
                    var rayVector =
                        new Point4D((float) ((opposite * 2 * i ) / _screen.Width - opposite), (float) ((opposite * 2 * j) / _screen.Height - opposite), 1) -
                        camera.Position;

                    var ray = new Ray(rayVector, camera.Position);

                    var colour = SendRay(scene, ray, 1.0f);

                    pixels[i,j] = new AsciiPixel(colour);
                }
            }

            return new Frame(pixels);
        }

        public byte SendRay(Scene scene, Ray ray, float total)
        {
            var triangle = scene.GetIntersectingTriangle(ray);

            foreach (var light in scene.Lights)
            {
            }

            return 0;

        }

        public byte SendTransparencyRay(Scene scene, Ray ray, float percentOfAffection)
        {
            return percentOfAffection * ()
        }

        public bool DoesLightAffect(Light scene, Ray ray)
        {
            return 0;
        }

    }
}