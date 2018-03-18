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
        public Vector Direction { get; }
        public Point4D Start { get; }
        public float Value { get; }

        public Ray(Vector direction, Point4D start, float value)
        {
            Direction = direction;
            Start = start;
            Value = value;
        }
    }
}
