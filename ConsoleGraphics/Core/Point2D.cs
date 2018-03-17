using System;

namespace ConsoleGraphics.Core
{
    public class Point2D
    {
        private float x;
        private float y;
        public Point2D(float x, float y) { this.x = x; this.y = y; }
        public Point2D() { }

        public float X { get { return x; } set { x = value; } }
        public float Y { get { return y; } set { y = value; } }

        public float Magnitude { get { return (float)Math.Sqrt(this.X * this.X + this.Y * this.Y); } }

        public static float Distance(Point2D a, Point2D b)
        {
            return (a - b).Magnitude;
        }

        public static Point2D operator +(Point2D p1, Point2D p2)
        {
            return new Point2D(p1.X + p2.X, p1.Y + p2.Y);
        }
        public static Point2D operator -(Point2D p1, Point2D p2)
        {
            return new Point2D(p1.X - p2.X, p1.Y - p2.Y);
        }

        public static Point2D CenterPoint(Point2D a, Point2D b)
        {
            return new Point2D(a.X + (-a.X + b.X) / 2, a.Y + (-a.Y + b.Y) / 2);
        }
    }
}
