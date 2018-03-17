using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGraphics.Core
{
    public class Triangle2
    {
        private readonly Point2 _p1;
        private readonly Point2 _p2;
        private readonly Point2 _p3;
        private readonly Func<float, float, float> _edge1;
        private readonly Func<float, float, float> _edge2;
        private readonly Func<float, float, float> _edge3;

        public Triangle2(Point2 p1, Point2 p2, Point2 p3)
        {
            _p1 = p1;
            _p2 = p2;
            _p3 = p3;
            _edge1 = GetEdgeEquation(p1, p2);
            _edge2 = GetEdgeEquation(p2, p3);
            _edge3 = GetEdgeEquation(p3, p1);
        }

        private Point2 GetInsidePoint()
        {
            return new Point2((_p1.X+_p2.X+_p3.X)/3, (_p1.Y + _p2.Y + _p3.Y) / 3, 0);
        }

        private Func<float, float, float> GetEdgeEquation(Point2 p1, Point2 p2)
        {
            float Func(float x, float y) => (p1.Y - p2.Y) * x + (p1.X - p2.X) * y + (p1.X * p2.Y - p2.X * p1.Y);
            var pt = GetInsidePoint();
            if (Func(pt.X, pt.Y) > 0)
                return Func;

            return (x,y) => -Func(x,y);
        }

        public bool IsContained(Point2 pt)
        {
            return _edge1(pt.X, pt.Y) > 0 && _edge2(pt.X, pt.Y) > 0 && _edge3(pt.X, pt.Y) > 0;
        }
    }

    public class Point2
    {
        private readonly byte _colour;
        public float X { get; }
        public float Y { get; }

        public Point2(float x, float y, byte colour)
        {
            _colour = colour;
            X = x;
            Y = y;
        }
    }
}
