using System;

namespace ConsoleGraphics.Core
{
    public class Triangle2D
    {
        private Point2D v1, v2, v3;

        public Triangle2D(Point2D v1, Point2D v2, Point2D v3)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
        }
        public Triangle2D() { }
        public Point2D V1 { get { return v1; } }
        public Point2D V2 { get { return v2; } }
        public Point2D V3 { get { return v3; } }

        public float Area {
            get
            {
                float lengthA, lengthB, lengthC;
                lengthA = Point2D.Distance(v1, v2);
                lengthB = Point2D.Distance(v1, v3);
                lengthC = Point2D.Distance(v2, v3);
                float s, sa, sb, sc;
                s = (lengthA + lengthB + lengthC) / 2;
                sa = (-lengthA + lengthB + lengthC) / 2;
                sb = (lengthA - lengthB + lengthC) / 2;
                sc = (lengthA + lengthB - lengthC) / 2;
                return (float)Math.Sqrt(s * sa * sb * sc);
            }
        }

        public Point2D Center
        {
            get
            {
                return new Point2D(
                     (v1.X + v2.X + v3.X) / 3,
                     (v1.Y + v2.Y + v3.Y) / 3
                    );
            }
        }

        public void drawTriangle(Tuple<int, ConsoleColor>[,] screen, int lightVal, ConsoleColor color)
        {
            drawTriangleRec(screen, lightVal, color, Area);
        }
        private void drawTriangleRec(Tuple<int, ConsoleColor>[,] screen, int lightVal, ConsoleColor color, float area)
        {
            if (area > .1)
            {
                Point2D v1_v2 = Point2D.CenterPoint(v1, v2);
                Point2D v1_v3 = Point2D.CenterPoint(v1, v3);
                Point2D v2_v3 = Point2D.CenterPoint(v2, v3);
                (new Triangle2D(v1, v1_v2, v1_v3)).drawTriangleRec(screen, lightVal, color, area/4);
                (new Triangle2D(v2, v1_v2, v2_v3)).drawTriangleRec(screen, lightVal, color, area/4);
                (new Triangle2D(v2_v3, v1_v2, v1_v3)).drawTriangleRec(screen, lightVal, color, area/4);
                (new Triangle2D(v3, v1_v3, v2_v3)).drawTriangleRec(screen, lightVal, color, area/4);
                //(new Triangle2D(CenterPoint(v1, v2), v3)).drawTriangle(screen, lightVal);
            }
            else
            {
                int x = (int)Math.Round((this.Center.X));
                int y = (int)Math.Round((this.Center.Y));
                //screen[(int)(this.Center.X), (int)(this.Center.Y)] = lightVal;
                if (Inbounds(screen, x, y))
                    screen[x, y] = new Tuple<int, ConsoleColor>(lightVal, color);
            }


        }

        public void drawTriangleShaded(Tuple<int, ConsoleColor>[,] screen, int l1, int l2, int l3, ConsoleColor color)
        {
            drawTriangleShadedRec(screen, l1,l2,l3, color, Area);
        }
        private void drawTriangleShadedRec(Tuple<int, ConsoleColor>[,] screen, int l1, int l2, int l3, ConsoleColor color, float area)
        {
            if (area > .1)
            {
                Point2D v1_v2 = Point2D.CenterPoint(v1, v2);
                Point2D v1_v3 = Point2D.CenterPoint(v1, v3);
                Point2D v2_v3 = Point2D.CenterPoint(v2, v3);
                (new Triangle2D(v1, v1_v2, v1_v3)).drawTriangleShadedRec(screen, l1, (l1 + l2)/2, (l1 + l3) / 2, color, area / 4);
                (new Triangle2D(v2, v1_v2, v2_v3)).drawTriangleShadedRec(screen, l2, (l1 + l2) / 2, (l2 + l3) / 2, color, area / 4);
                (new Triangle2D(v2_v3, v1_v2, v1_v3)).drawTriangleShadedRec(screen, (l2+l3)/2, (l1 + l2) / 2, (l1 + l3) / 2, color, area / 4);
                (new Triangle2D(v3, v1_v3, v2_v3)).drawTriangleShadedRec(screen, l3, (l1 + l3) / 2, (l2 + l3) / 2, color, area / 4);
                //(new Triangle2D(CenterPoint(v1, v2), v3)).drawTriangle(screen, lightVal);
            }
            else
            {
                int x = (int)Math.Round((this.Center.X));
                int y = (int)Math.Round((this.Center.Y));
                //screen[(int)(this.Center.X), (int)(this.Center.Y)] = lightVal;
                if (Inbounds(screen, x, y))
                    screen[x, y] = new Tuple<int, ConsoleColor>(l1, color);
            }

        }
        private bool Inbounds(Tuple<int, ConsoleColor>[,] screen, int x, int y)
        {
            return y >= 0 && y < (screen.Length / screen.GetLength(0)) && x >= 0 && x < screen.GetLength(0);
        }
    }
}
