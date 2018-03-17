using System;
using GraphicsProject.TinyIoC;

namespace ConsoleGraphics.Core
{
    public delegate Point3D GetNormal();

    public class Triangle3D : IEquatable<Triangle3D>, IApplyMatrix
    {
        private GetNormal normalFunc;
        private Vector4D _normal;
         
        public Triangle3D(Point3D v1, Point3D v2, Point3D v3)
        {
            V1 = v1;
            V2 = v2;
            V3 = v3;
            normalFunc = Point3D.Cross(V3 - V1, V2 - V1).Normalize;
        }
        public Triangle3D(Point3D v1, Point3D v2, Point3D v3, Point3D normal)
        {
            _normal = normal;
            normalFunc = normal.Normalize;
        }
        public Triangle3D() { }

        public Point3D V1 { get; }
        public Point3D V2 { get; }
        public Point3D V3 { get; }
        public Vector4D Normal => normalFunc();

        public Point3D Center => new Point3D(
            (V1.X + V2.X + V3.X) / 3,
            (V1.Y + V2.Y + V3.Y) / 3,
            (V1.Z + V2.Z + V3.Z) / 3
        );

        public Tuple<Point3D, Point3D, Point3D> getPoints()
        {
            return new Tuple<Point3D, Point3D, Point3D>(V1, V2, V3);
        }

        public static explicit operator Triangle2D(Triangle3D tri) // explicit byte to digit conversion operator
        {
            return new Triangle2D((Point2D) tri.V1, (Point2D) tri.V2, (Point2D) tri.V3);
        }


        /// <summary>
        /// returns a value from 0 - 255 based on how the given lightsource will light up the face
        /// 0 means its facing the entirely wrong way
        /// 127 means its perpendicular to the light
        /// 255 means its directly facing the light.
        /// </summary>
        /// <param name="lightSrc"></param>
        /// <returns></returns>
        /*public int Reflectivity(Light lightSrc)
        {
            Point3D normal = Normal;
            Point3D lightVector = (this.Center - lightSrc.Location);
            float dotProduct; // cosine of the angle between the normal and the lightsource (from -1 to 1)
            dotProduct = (normal * lightVector) / (lightVector.Magnitude * normal.Magnitude);

            float temp = (1 + dotProduct) / 2; //max(2/2) = 1, min = (0/2) = 0
            return (int)(temp * 255);
        }

        public float getLightValue(List<Light> lightSrc)
        {
            float lightVal = 0;
            foreach (Light l in lightSrc)
            {
                int reflect = Reflectivity(l);

                if (reflect >= 128) // 128 -> 255 means face normal is pointing towards lightsource in some way.
                {
                    float maxDistance = 1000 * l.Intensity;
                    float light_triangle_dist = (this.Center - l.Location).Magnitude;
                    //lightVal += l.Intensity * (reflect - 127) / (light_triangle_dist * light_triangle_dist);
                    //lightVal += 255 - (l.Intensity * (255 - reflect) * (1 / Math.Max(light_triangle_dist, 1)));
                    lightVal += (float)Math.Max(0, l.Intensity * ((reflect - 127) * 2) * -((light_triangle_dist - maxDistance) / maxDistance));
                    lightVal = Math.Min(255, lightVal); // don't go over 255!

                }
            }
            return Math.Min(255,lightVal+15);
        }*/

        public bool HasVertex(Point3D v)
        {
            return V1.Equals(v) || V2.Equals(v) || V3.Equals(v);
        }
     
        public Triangle3D Translate(Point3D amount)
        {
            V1.Translate(amount);
            V2.Translate(amount);
            V3.Translate(amount);
            return this;
        }
        public Triangle3D Scale(Point3D amount)
        {
            V1.Scale(amount);
            V2.Scale(amount);
            V3.Scale(amount);
            return this;
        }
        public void TranslateTo(Point3D location)
        {
            Point3D amount = location - Center;
            Translate(amount);
        }
        public void RotateLocal(Point3D theta)
        {
            Point3D oldCenter = Center;
            TranslateTo(new Point3D(0, 0, 0));
            V1.Rotate(theta);
            V2.Rotate(theta);
            V3.Rotate(theta);
            TranslateTo(oldCenter);
        }
        public void RotateGlobal(Point3D theta)
        {
            V1.Rotate(theta);
            V2.Rotate(theta);
            V3.Rotate(theta);
        }
        public Triangle2D Projection(float distance)
        {
            return new Triangle2D(V1.Projection(distance), V2.Projection(distance), V3.Projection(distance));
        }
        public bool Equals(Triangle3D other)
        {
            return V1.Equals(other.V1) && V2.Equals(other.V2) && V3.Equals(other.V3);
        }

        public dynamic Apply(Matrix m)
        {
            return new Triangle3D(V1.Apply(m), V2.Apply(m), V3.Apply(m));
            
        }
    }
}
