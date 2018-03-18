using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleGraphics.Core;
using ConsoleGraphics.Core.Light;
using ConsoleGraphics.Display;
using ConsoleGraphics.Extensions;

namespace ConsoleGraphics.Render
{
    public class RayTracer : IRender
    {
        private readonly Screen _screen;
        private readonly ILuminosityCalculator _luminosityCalculator;
        private readonly IRayGenerator _rayGenerator;

        public RayTracer(Screen screen,
            ILuminosityCalculator luminosityCalculator,
            IRayGenerator rayGenerator)
        {
            _screen = screen;
            _luminosityCalculator = luminosityCalculator;
            _rayGenerator = rayGenerator;
        }

        public Frame RenderFrame(Scene scene, Camera camera)
        {
            var pixels = new Pixel[_screen.Width, _screen.Height];
            var opposite = 2 * Math.Tan(camera.ViewAngle);
            var rays = new Stack<Ray>();

            for (var i = 0; i < _screen.Width; i++)
            {
                for (var j = 0; j < _screen.Height; j++)
                {
                    var rayVector =
                        new Point4D((float) ((opposite * 2 * i ) / _screen.Width - opposite), (float) ((opposite * 2 * j) / _screen.Height - opposite), 1) -
                        camera.Position;

                    var ray = new Ray(rayVector, camera.Position, 1.0f);

                    var colour = SendRay(scene, ray, 3);  //todo: Get 3 (max ray depth) from appsettings

                    //pixels[i,j] = new AsciiPixel(colour);
                }
            }

            return new Frame(pixels);
        }

        internal Rgba SendRay(Scene scene, Ray ray, int depth)
        {
            var intersection = scene.GetIntersection(ray);
            
            if (intersection == null)
                return Rgba.NoLight;

            var totalLight = scene.Lights.Sum((light) => {
                var lightval = _luminosityCalculator.Luminosity(intersection.IntersectionPoint, intersection.Normal, light);
                if (light is AmbientLight)
                    return lightval;
                if (light is IHaveLocation lightAsLocation)
                {
                    var lightIntersection =
                        scene.GetIntersection(new Ray(lightAsLocation.Location - ray.Start, ray.Start, 0));

                    if (lightIntersection == null || (lightIntersection.IntersectionPoint - ray.Start).Magnitude >
                        (lightAsLocation.Location - ray.Start).Magnitude) //then light affects it
                    {
                        return lightval;
                    }
                }
                return lightval;
            });

            var nextRays = _rayGenerator.GenerateNextRays(ray, intersection);

            return nextRays.Aggregate(totalLight, 
                (current, nextRay) => current + SendRay(scene, nextRay, depth - 1));
        }
    }

    public class RayGenerator : IRayGenerator
    {
        public IEnumerable<Ray> GenerateNextRays(Ray currentRay, RayIntersection intersection)
        {
            var rays = new List<Ray>();

            var reflectionRay = new Ray(intersection.Normal.GetReflectionVector(intersection.Normal),
                intersection.IntersectionPoint, currentRay.Value * intersection.Refectivity);
            rays.Add(reflectionRay);

            if (intersection.Transparency > .05) //todo: Set .05 in appsettings
            {
                var tranparentRay = new Ray(currentRay.Direction, intersection.IntersectionPoint,
                    currentRay.Value * intersection.Transparency);
                rays.Add(tranparentRay);
            }

            return rays;
        }
    }
}