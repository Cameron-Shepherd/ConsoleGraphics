using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGraphics.Core
{
    public abstract class Vector : IApplyMatrix
    {
        public abstract float X { get; }
        public abstract float Y { get; }
        public abstract float Z { get; }
        public abstract float Magnitude { get; }

        public abstract Vector Normalize();
        public abstract Vector Translate(Vector amount);
        public abstract Vector Rotate(Vector vectorTheta);
        public abstract Vector Rotate(float theta);

        public abstract Vector Add(Vector b);
        public static Vector operator +(Vector v1, Vector v2)
        {
            return v1.Add(v2);
        }

        public abstract Vector Negate();
        public static Vector operator -(Vector v1)
        {
            return v1.Negate();
        }

        public abstract Vector Cross(Vector b);
        public static Vector operator ^(Vector v1, Vector v2)
        {
            return v1.Cross(v2);
        }

        public abstract float Dot(Vector b);
        public static float operator *(Vector v1, Vector v2)
        {
            return v1.Dot(v2);
        }

        public abstract Vector Scale(float scale);
        public static Vector operator *(float scale, Vector v1)
        {
            return v1.Scale(scale);
        }
        public static Vector operator *(Vector v1, float scale)
        {
            return v1.Scale(scale);
        }

        public static Vector operator *(Vector v, Matrix m)
        {
            return v.Apply(m);
        }

        public abstract dynamic Apply(Matrix a);
    }
}
