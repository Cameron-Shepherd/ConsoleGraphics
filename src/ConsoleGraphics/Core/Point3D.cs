using System;
using System.Collections.Generic;

namespace ConsoleGraphics.Core
{
    public class Point3D: IEquatable<Point3D>, IApplyMatrix
    {
        private Point3D vertexNorm;
        private Vector4D point;

        public Point3D(float x, float y, float z)
        {
            point = new Vector4D(x, y, z, 1);
        }
        public Point3D(float x, float y, float z, Point3D vn)
        {
            point = new Vector4D(x, y, z, 1);
            vertexNorm = vn;
        }

        public Point3D VN { get { return vertexNorm; } set { vertexNorm = value; } }
        public float Magnitude => (float)Math.Sqrt(X * X + Y * Y + Z * Z);

        public float X { get { return point[0]; } }
        public float Y { get { return point[1]; } }
        public float Z { get { return point[2]; } }

        public Point3D Vn { get; }

        public dynamic Apply(Matrix m)
        {
            return point * m;
        }

        public static explicit operator Point2D(Point3D pt) // explicit byte to digit conversion operator
        {
            return new Point2D(pt.X, pt.Y);
        }

        public float Length()
        {
            return (float)Math.Sqrt((this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z));
        }

        public int Reflectivity(Light.Light lightSrc, Point3D lookVect)
        {
            Point3D lightVector = (this - lightSrc.Location);
            Point3D locNormal = vertexNorm;

            //Diffuse
            float dotProduct; // cosine of the angle between the normal and the lightsource (from -1 to 1)
            dotProduct = (locNormal * lightVector) / (lightVector.Magnitude * locNormal.Magnitude);
            float temp = (1 + dotProduct) / 2; //max(2/2) = 1, min = (0/2) = 0
            //float temp = 0;

            //Specular
            Point3D reflectVect = lookVect - 2 * (Point3D.Dot(lookVect, locNormal) * locNormal);
            lightVector.Normalize();
            reflectVect.Normalize();
            if (Point3D.Dot(lightVector, reflectVect) < -.85f)
                temp = 1/lightSrc.Intensity;

            return (int)(temp * 255);
        }

        public float getLightValue(List<Light.Light> lightSrc, Point3D lookVect)
        {
            float lightVal = 0;
            foreach (Light.Light l in lightSrc)
            {
                int reflect = Reflectivity(l, lookVect);

                if (reflect >= 128) // 128 -> 255 means face normal is pointing towards lightsource in some way.
                {
                    float maxDistance = 1000 * l.Intensity;
                    float light_triangle_dist = (this - l.Location).Magnitude;
                    //lightVal += l.Intensity * (reflect - 127) / (light_triangle_dist * light_triangle_dist);
                    //lightVal += 255 - (l.Intensity * (255 - reflect) * (1 / Math.Max(light_triangle_dist, 1)));
                    lightVal += (float)Math.Max(0, l.Intensity * ((reflect - 127) * 2) * -((light_triangle_dist - maxDistance) / maxDistance));
                    lightVal = Math.Min(255, lightVal); // don't go over 255!

                }
            }
            return Math.Min(255, lightVal + 15);
        }

        public void Rotate(Point3D theta)
        {
            //rotate z axis (change x/y values)
            float temp = (float)(this.X * Math.Cos(theta.z) - this.Y * Math.Sin(theta.Z));
            this.Y = (float)(this.X * Math.Sin(theta.z) + this.Y * Math.Cos(theta.Z));
            this.X = temp;

            //rotate y axis (change z/x values)
            temp = (float)(this.X * Math.Cos(theta.Y) - this.Z * Math.Sin(theta.Y));
            this.Z = (float)(this.X * Math.Sin(theta.Y) + this.Z * Math.Cos(theta.Y));
            this.X = temp;

            //rotate x axis (change z/y values)
            temp = (float)(this.Z * Math.Cos(theta.X) - this.Y * Math.Sin(theta.X));
            this.Y = (float)(this.Z * Math.Sin(theta.X) + this.Y * Math.Cos(theta.X));
            this.Z = temp;

            if(vertexNorm != null)
                vertexNorm.Rotate(theta);
        }
        //-----------------------------------------------------------------------------------------------------------
        public static float Dot(Point3D a, Point3D b)
        {
            return (a.X * b.X + a.Y * b.Y + a.Z * b.Z);
        }
        public float Dot(Point3D b)
        {
            return (this.X * b.X + this.Y * b.Y + this.Z * b.Z);
        }

        public static Point3D Cross(Point3D a, Point3D b)
        {
            return new Point3D((a.Y * b.Z - a.Z * b.Y), (a.Z * b.X - a.X * b.Z), (a.X * b.Y - a.Y * b.X));
            Point3D.Dot(a, b);
        }
        public static float Distance(Point3D a, Point3D b)
        {
            return (b - a).Magnitude;
        }
        public static float operator *(Point3D p1, Point3D p2)
        {
            return Dot(p1, p2);
        }
        public static Point3D operator ^(Point3D p1, Point3D p2)
        {
            return Cross(p1, p2);
        }
        public static Point3D operator +(Point3D p1, Point3D p2)
        {
            return new Point3D(p1.X+p2.X, p1.Y+p2.Y, p1.Z+p2.Z);
        }
        //public static Point3D operator -(Point3D p1, Point3D p2)
        //{
        //    return new Point3D(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        //}
        public static Point3D operator -(Point3D p1)
        {
            return new Point3D(-p1.X, -p1.Y, -p1.Z);
        }

        public static Point3D operator *(Point3D p1, float scalar)
        {
            return new Point3D(p1.X * scalar, p1.Y * scalar, p1.Z * scalar);
        }

        public static Point3D operator *(float scalar, Point3D p1)
        {
            return new Point3D(p1.X * scalar, p1.Y * scalar, p1.Z * scalar);
        }

        public static Point3D operator /(Point3D p1, float scalar)
        {
            return new Point3D(p1.X / scalar, p1.Y / scalar, p1.Z / scalar);
        }

        public static Vector4D operator -(Point3D v1, Point3D v2)
        {
            return new Vector4D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z, 1);
        }

        //--------------------------------------------------------------------------------------------------------
        public Point2D Projection(float distance)
        {
            return new Point2D(this.X * distance / (distance - this.Z), this.Y * distance / (distance - this.Z));
        }

        public static Point3D CenterPoint(Point3D a, Point3D b)
        {
            return new Point3D(a.X + (-a.X + b.X) / 2, a.Y + (-a.Y + b.Y) / 2, a.Z + (-a.Z + b.Z) / 2);
        }

        public Point3D Normalize()
        {
            float length = Length();
            this.X = this.X / length;
            this.Y = this.Y / length;
            this.Z = this.Z / length;
            return this;
        }

        public Point3D Translate(Point3D amount)
        {
            this.X += amount.X;
            this.Y += amount.Y;
            this.Z += amount.Z;
            return this;
        }
        public Point3D Scale(Point3D amount)
        {
            this.X *= amount.X;
            this.Y *= amount.Y;
            this.Z *= amount.Z;
            return this;
        }
        public bool Equals(Point3D other)
        {
           // return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
            return this == other;
        }
        public Point3D Clone()
        {
            return new Point3D(this.X, this.Y, this.Z,this.vertexNorm);
        }

    }
}
