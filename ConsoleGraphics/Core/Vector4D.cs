using System;
using ConsoleGraphics.Extensions;

namespace ConsoleGraphics.Core
{
    public class Vector4D : Vector
    {
        private Matrix m;
        public override float X => m[0, 0];
        public override float Y => m[0, 1];
        public override float Z => m[0, 2];
        public float W => m[0, 3];

        public override float Magnitude => (float)Math.Sqrt(X * X + Y * Y + Z * Z);

        public Vector4D (float x, float y, float z, float w)
        {
            m = new Matrix( new[]{x,y,z,w} );
        }
        public override Vector Add(Vector b)
        {
            var vector = CheckType(b);
            return new Vector4D(X + vector.X, Y + vector.Y, Z + vector.Z, W + vector.W);
        }

        public override dynamic Apply(Matrix a)
        {
            var asMatrix = new Matrix(this);
            return asMatrix.Multiply(a);
        }

        public override Vector Cross(Vector b)
        {
            return new Vector4D((Y * b.Z - Z * b.Y), (Z * b.X - X * b.Z), (X * b.Y - Y * b.X), 1);
        }

        public override float Dot(Vector b)
        {
            return (X * b.X + Y * b.Y + Z * b.Z);
        }

        public override Vector Negate()
        {
            return new Vector4D(-X, -Y, -Z, W);
        }

        public override Vector Normalize()
        {
            float length = Magnitude;
            return new Vector4D(X / length, Y / length, Z / length, W);
        }

        public override Vector Rotate(Vector vectorTheta)
        {
            return this.Rotate4D(vectorTheta);
        }

        public override Vector Rotate(float theta)
        {
            throw new NotImplementedException();
        }

        public override Vector Scale(float scale)
        {
            return new Vector4D(X * scale, Y * scale, Z * scale, W);
        }

        public override Vector Translate(Vector amount)
        {
            return Add(amount);
        }

        public static explicit operator Vector4D(Matrix b)
        {
            if (b.Cols == 1 && b.Rows == 4)
                return new Vector4D(b[0, 0], b[0, 1], b[0, 2], b[0, 3]);

            throw new ArgumentException();
        }

        public float this[int row] => m[row, 0];

        private Vector4D CheckType(Vector v)
        {
            if (v is Vector4D d)
                return d;

            throw new FormatException();
        }
    }
}
