using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleGraphics.Core;

namespace ConsoleGraphics.Render
{
    public class RayIntersection
    {
        public Vector Normal { get; }
        public Point4D IntersectionPoint { get; }
        public float Transparency { get; }
        public float Refectivity { get; }

        public RayIntersection(Vector normal, Point4D intersectionPoint, float transparency, float refectivity)
        {
            Normal = normal;
            IntersectionPoint = intersectionPoint;
            Transparency = transparency;
            Refectivity = refectivity;
        }
    }
}
