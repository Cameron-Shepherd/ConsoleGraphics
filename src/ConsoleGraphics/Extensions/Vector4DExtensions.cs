using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleGraphics.Core;

namespace ConsoleGraphics.Extensions
{
    public static class Vector4DExtensions
    {
        public static Vector4D Rotate4D(this Vector4D a, Vector rotateVector)
        {
            var vMatrix = new Matrix(a);
            var rotateX = Matrix.RotationX(rotateVector.X);
            var rotateY = Matrix.RotationY(rotateVector.Y);
            var rotateZ = Matrix.RotationZ(rotateVector.Z);
            return (Vector4D)(rotateX * rotateY * rotateZ * vMatrix);
        }
    }
}
