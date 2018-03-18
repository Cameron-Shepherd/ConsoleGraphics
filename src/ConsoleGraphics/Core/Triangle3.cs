using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGraphics.Core
{
    public class Triangle3
    {
        public Point4D P1 { get; }
        public Point4D P2 { get; }
        public Point4D P3 { get; }
        public Vector4D Normal { get; }

        public Triangle3(Point4D p1, Point4D p2, Point4D p3)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            Normal = new Vector4D(0,0,0,0);
        }

        public static explicit operator Triangle2(Triangle3 tri) // explicit byte to digit conversion operator
        {
            return new Triangle2((Point2)tri.P1, (Point2)tri.P2, (Point2)tri.P3);
        }
    }
}
