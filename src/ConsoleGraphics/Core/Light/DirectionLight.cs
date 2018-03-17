using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGraphics.Core.Light
{
    public class DirectionLight : Light
    {
        public Vector Direction { get; }

        public DirectionLight(Vector direction, float intensity) : base(intensity)
        {
            Direction = direction;
        }
    }
}
