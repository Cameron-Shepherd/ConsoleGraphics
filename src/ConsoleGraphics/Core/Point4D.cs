using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGraphics.Core
{
    public class Point4D
    {
        private readonly Matrix m;
        public float X => m[0, 0];
        public float Y => m[1, 0];
        public float Z => m[2, 0];
        public float W => m[3, 0];

        public Point4D(float x, float y, float z)
        {
            m = new Matrix(new[] {x, y, z, 1});
        }

        public Point2 ToPoint2(byte colour)
        {
            return new Point2(X, Y, colour);
        }

        public static Vector4D operator -(Point4D p1, Point4D p2)
        {
            return new Vector4D(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z, 0);
        }

        public Point4D AddVector(Vector v)
        {
            return new Point4D(X + v.X, Y + v.Y, Z + v.Z);
        }

        public static Point4D operator +(Point4D p1, Vector4D v)
        {
            return p1.AddVector(v);
        }

        public static Point4D operator +(Vector4D v, Point4D p1)
        {
            return p1.AddVector(v);
        }
    }
}
