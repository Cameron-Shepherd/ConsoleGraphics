using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleGraphics.Core;

namespace ConsoleGraphics.Render
{
    public class Ray
    {
        public Vector4D Direction { get; }
        public Point4D Start { get; }

        public Ray(Vector4D direction, Point4D start)
        {
            Direction = direction;
            Start = start;
        }
    }
}
