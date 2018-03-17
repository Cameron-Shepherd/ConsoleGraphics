using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace ConsoleGraphics.Core
{
    public class Mesh : IApplyMatrix
    {
        // indexed by each vertex, gives you the list of faces that use that vertex
        private Dictionary<Point3D, List<Triangle3D>> vertexList;

        private List<Point3D> verticies;
        private bool shaded;
            
            // indexed by the face, gives you a list of vertexes in the face
        private Dictionary<Triangle3D, Tuple<Point3D, Point3D, Point3D>> faceList;
        private Material material;
        public Mesh(List<Point3D> verts, List<Triangle3D> faces, Material m)
        {
            this.vertexList = new Dictionary<Point3D, List<Triangle3D>>();
            this.faceList = new Dictionary<Triangle3D, Tuple<Point3D, Point3D, Point3D>>();
            this.verticies = new List<Point3D>();
            this.material = m;
            GenerateLists(verts, faces);
            shaded = true;
        }
        public Mesh(Material m)
        {
            this.vertexList = new Dictionary<Point3D, List<Triangle3D>>();
            this.faceList = new Dictionary<Triangle3D, Tuple<Point3D, Point3D, Point3D>>();
            this.material = m;
            this.verticies = new List<Point3D>();
            shaded = false;
        }
        public Mesh()
        {
            vertexList = new Dictionary<Point3D, List<Triangle3D>>();
            faceList = new Dictionary<Triangle3D, Tuple<Point3D, Point3D, Point3D>>();
            verticies = new List<Point3D>();
            shaded = false;
        }
        public int VertexCount { get { return vertexList.Count; } }
        public int FaceCount { get { return faceList.Count; } }
        public Material Material { get { return material; } }
        public bool Shaded { get { return shaded; } }
        public Point3D Center
        {
            get
            {
                float tempX = 0;
                float tempY = 0;
                float tempZ = 0;
                foreach (KeyValuePair<Point3D, List<Triangle3D>> p in vertexList)
                {
                    tempX += p.Key.X;
                    tempY += p.Key.Y;
                    tempZ += p.Key.Z;
                }
                tempX /= VertexCount;
                tempY /= VertexCount;
                tempZ /= VertexCount;
                return new Point3D(tempX, tempY, tempZ);
            }
        }

        public dynamic Apply(Matrix m)
        {
            return verticies.Select(x => x.Apply(m));
        }

        public IEnumerable<Triangle3D> Faces => faceList.Keys.AsEnumerable();

        private void GenerateLists(List<Point3D> verts, List<Triangle3D> faces)
        {
            // add the vertexes to the dictionary
            foreach (var v in verts)
            {
                verticies.Add(new Point3D(v.X, v.Y, v.Z));
                vertexList.Add(v, new List<Triangle3D>());
            }
            foreach (var t in faces)
            {
                faceList.Add(t, t.getPoints());
                vertexList[t.V1].Add(t);
                vertexList[t.V2].Add(t);
                vertexList[t.V3].Add(t);
            }

            // Generate vertex normals
            foreach (KeyValuePair<Point3D, List<Triangle3D>> pts in vertexList)
            {
                Point3D vertexNorm = new Point3D(0, 0, 0);
                foreach (Triangle3D t in pts.Value) // look at each triangle
                {
                    vertexNorm += t.Normal;
                }
                pts.Key.VN = vertexNorm.Normalize();
            }
        }

        /// <summary>
        /// return a deep copy of all the faces 
        /// </summary>
        /// <returns></returns>
        //public List<Triangle3D> Faces()
        //{
        //    List<Triangle3D> temp = new List<Triangle3D>();
        //    foreach (KeyValuePair<Triangle3D, Tuple<Point3D, Point3D, Point3D>> d in faceList)
        //    {
        //        temp.Add(new Triangle3D(d.Key.V1.Clone(), d.Key.V2.Clone(), d.Key.V3.Clone()));
        //    }
        //    return temp;
        //}

        /*public void Draw(int[,] screen, float dist, List<Light> lightSrc)
        {
            foreach (KeyValuePair<Triangle3D, Tuple<Point3D, Point3D, Point3D>> d in faceList)
            {
                d.Key.Draw(screen, dist, lightSrc);
            }
        }*/
        /// <summary>
        /// local rotation around mesh center
        /// </summary>
        /// <param name="delta"></param>
        public void Rotate(Point3D theta)
        {
            Point3D oldCenter = Center;
            TranslateTo(new Point3D(0, 0, 0));
            foreach (KeyValuePair<Point3D, List<Triangle3D>> p in vertexList)
            {
                p.Key.Rotate(theta);
            }
            TranslateTo(oldCenter);
        }

        public void Translate(Point3D amount)
        {
            foreach (KeyValuePair<Point3D, List<Triangle3D>> p in vertexList)
            {
                p.Key.Translate(amount);
            }
        }

        public void TranslateTo(Point3D location)
        {
            Point3D amount = location - Center;
            Translate(amount);
        }

        public void Scale(float amt)
        {
            Point3D curr = this.Center;
            this.TranslateTo(new Point3D(0, 0, 0));
            foreach (KeyValuePair<Point3D, List<Triangle3D>> p in vertexList)
            {
                p.Key.Scale(new Point3D(amt, amt, amt));
            }
            this.TranslateTo(curr);
        }

        public static Mesh GeneratePyramid(float width, float depth, float height, Point3D loc, Material mat)
        {
            Mesh m;
            Point3D a = new Point3D(loc.X - width / 2, loc.Y - height / 2, loc.Z);
            Point3D b = new Point3D(loc.X + width / 2, loc.Y - height / 2, loc.Z);
            Point3D c = new Point3D(loc.X + width / 2, loc.Y + height / 2, loc.Z);
            Point3D d = new Point3D(loc.X - width / 2, loc.Y + height / 2, loc.Z);
            Point3D e = new Point3D(loc.X, loc.Y, loc.Z + height / 2);

            Triangle3D tri = new Triangle3D(d, a, e);
            Triangle3D tri2 = new Triangle3D(a, b, e);
            Triangle3D tri3 = new Triangle3D(b, c, e);
            Triangle3D tri4 = new Triangle3D(c, d, e);
            Triangle3D tri5 = new Triangle3D(c, a, d);
            Triangle3D tri6 = new Triangle3D(b, a, c);

            List<Point3D> points = new List<Point3D>();
            List<Triangle3D> triangles = new List<Triangle3D>();
            points.Add(a);
            points.Add(b);
            points.Add(c);
            points.Add(d);
            points.Add(e);
            triangles.Add(tri);
            triangles.Add(tri2);
            triangles.Add(tri3);
            triangles.Add(tri4);
            triangles.Add(tri5);
            triangles.Add(tri6);
            m = new Mesh(points, triangles, mat);
            m.shaded = false;
            return m;
        }

        public static Mesh GenerateSphere(float radius, Point3D loc, Material mat)
        {
            return GenerateSphere(radius, loc, mat, 20, 20);
        }
        private static Mesh GenerateSphere(float radius, Point3D loc, Material mat, int detail, int levels)
        {
            //int detail = 30;
            Point3D top = new Point3D(0, radius, 0);
            Point3D bottom = new Point3D(0, -radius, 0);
            //int levels = 9;
            int centerlevel = levels / 2;
            List<Point3D> totalPointList = new List<Point3D>();
            totalPointList.Add(top);
            totalPointList.Add(bottom);

            List<Point3D> prevPointList = null;
            List<Triangle3D> triList = new List<Triangle3D>();
            //int currLevel = 1;
            List<Point3D> pointList = new List<Point3D>();

            for (int currLevel = 1; currLevel < levels - 1; currLevel++)
            {
                pointList = new List<Point3D>();
                float angle = 0;
                if (currLevel % 2 == 0) // offset the point by half
                    angle = (float)Math.PI / detail;

                //float height = (centerlevel - currLevel) * (radius*2 / (levels-1));
                float height = (float)Math.Cos(currLevel * (Math.PI / (levels - 1))) * radius;
                float currRadius = (float)Math.Sqrt((radius * radius) - (height * height));
                for (int i = 0; i < detail; i++)
                {
                    pointList.Add(new Point3D((float)Math.Cos(angle) * currRadius, height, (float)Math.Sin(angle) * currRadius));
                    angle += (float)Math.PI * 2 / detail;
                }
                if (currLevel == 1) // connect these points to top point
                {
                    for (int i = 0; i < detail; i++)
                    {
                        if (i != detail - 1)
                            triList.Add(new Triangle3D(pointList[i + 1], pointList[i], top));
                        else
                            triList.Add(new Triangle3D(pointList[0], pointList[i], top));
                    }
                    totalPointList.AddRange(pointList); // add those points to total list
                    prevPointList = pointList; // set the list as the previous
                }
                else if (currLevel > 1 && currLevel <= levels - 2) // some middle row
                {
                    for (int i = 0; i < detail; i++)
                    {
                        if (i != detail - 1)
                        {
                            triList.Add(new Triangle3D(pointList[i + 1], pointList[i], prevPointList[i + 1]));
                            triList.Add(new Triangle3D(prevPointList[i], prevPointList[i + 1], pointList[i]));
                        }
                        else
                        {
                            triList.Add(new Triangle3D(pointList[0], pointList[i], prevPointList[0]));
                            triList.Add(new Triangle3D(prevPointList[i], prevPointList[0], pointList[i]));
                        }
                    }

                    totalPointList.AddRange(pointList); // add those points to total list
                    prevPointList = pointList; // set the list as the previous

                }
                if (currLevel == levels - 2) // connect these Points to bottom point
                {
                    for (int i = 0; i < detail; i++)
                    {
                        if (i != detail - 1)
                            triList.Add(new Triangle3D(pointList[i], pointList[i + 1], bottom));
                        else
                            triList.Add(new Triangle3D(pointList[i], pointList[0], bottom));
                    }
                }


            } // end forloop
            Mesh m = new Mesh(totalPointList, triList, mat);
            m.shaded = true;
            m.Translate(loc);
            return m;
        } // end function


        public static Mesh GenerateCylinder(float height, float radius, Point3D loc, Material mat)
        {
            Point3D top = new Point3D(0, height, 0);
            Point3D bottom = new Point3D(0, 0, 0);
            int levels = 1;
            int detail = 30;
            // int centerlevel = levels / 2;
            List<Point3D> totalPointList = new List<Point3D>();
            totalPointList.Add(top);
            totalPointList.Add(bottom);

            List<Point3D> prevPointList = null;
            List<Triangle3D> triList = new List<Triangle3D>();
            //int currLevel = 1;
            List<Point3D> pointList = new List<Point3D>();


            for (int currLevel = 0; currLevel <= levels; currLevel++)
            {
                pointList = new List<Point3D>();
                float angle = 0;
                if (currLevel % 2 == 0) // offset the point by half
                    angle = (float)Math.PI / detail;

                //float height = (centerlevel - currLevel) * (radius*2 / (levels-1));
                float currHeight = currLevel * height / levels;
                for (int i = 0; i < detail; i++)
                {
                    pointList.Add(new Point3D((float)Math.Cos(angle) * radius, currHeight, (float)Math.Sin(angle) * radius));
                    angle += (float)Math.PI * 2 / detail;
                }
                if (currLevel == 0)
                {
                    for (int i = 0; i < detail; i++)
                    {
                        if (i != detail - 1)
                            triList.Add(new Triangle3D(pointList[i + 1], top, pointList[i]));
                        else
                            triList.Add(new Triangle3D(pointList[0], top, pointList[i]));
                    }
                    totalPointList.AddRange(pointList); // add those points to total list
                    prevPointList = pointList; // set the list as the previous
                }
                else
                {
                    for (int i = 0; i < detail; i++)
                    {
                        if (i != detail - 1)
                        {
                            triList.Add(new Triangle3D(pointList[i + 1], prevPointList[i + 1], pointList[i]));
                            triList.Add(new Triangle3D(prevPointList[i], pointList[i], prevPointList[i + 1]));
                        }
                        else
                        {
                            triList.Add(new Triangle3D(pointList[0], prevPointList[0], pointList[i]));
                            triList.Add(new Triangle3D(prevPointList[i], pointList[i], prevPointList[0]));
                        }
                    }

                    totalPointList.AddRange(pointList); // add those points to total list
                    prevPointList = pointList; // set the list as the previous
                }

                if (currLevel == levels)
                {
                    for (int i = 0; i < detail; i++)
                    {
                        if (i != detail - 1)
                            triList.Add(new Triangle3D(pointList[i], bottom, pointList[i + 1]));
                        else
                            triList.Add(new Triangle3D(pointList[i], bottom, pointList[0]));
                    }
                }
            }
            Mesh m = new Mesh(totalPointList, triList, mat);
            m.Translate(loc);
            m.shaded = true;
            return m;
        }
        public static Mesh GenerateRectangle(float width, float depth, float height, Point3D loc, Material mat)
        {
            List<Triangle3D> triangles = new List<Triangle3D>();
            List<Point3D> points;

            Point3D a = new Point3D(0, 0, 0);
            Point3D b = new Point3D(width, 0, 0);
            Point3D c = new Point3D(0, height, 0);
            Point3D d = new Point3D(width, height, 0);
            Point3D aa = new Point3D(0, 0, depth);
            Point3D bb = new Point3D(width, 0, depth);
            Point3D cc = new Point3D(0, height, depth);
            Point3D dd = new Point3D(width, height, depth);

            triangles.Add(new Triangle3D(c, d, a));
            triangles.Add(new Triangle3D(b, a, d));
            triangles.Add(new Triangle3D(bb, b, dd));
            triangles.Add(new Triangle3D(d, dd, b));
            triangles.Add(new Triangle3D(c, cc, d));
            triangles.Add(new Triangle3D(dd, d, cc));
            triangles.Add(new Triangle3D(aa, cc, a));
            triangles.Add(new Triangle3D(c, a, cc));
            triangles.Add(new Triangle3D(b, a, bb));
            triangles.Add(new Triangle3D(aa, bb, a));
            triangles.Add(new Triangle3D(dd, cc, bb));
            triangles.Add(new Triangle3D(aa, bb, cc));

            points = new List<Point3D>() { a, b, c, d, aa, bb, cc, dd };
            Mesh m = new Mesh(points, triangles, mat);
            m.TranslateTo(loc);
            m.shaded = false;
            return m;
        }

        public List<Point3D> getVerticies() {
            return verticies;
        }
       /* public static Mesh generateComplexObj(List<Point3D> v, List<Point3D> vn, List<Point3D> f, Point3D loc, Material mat)
        {
            Mesh m;
            List<Triangle3D> triangles = new List<Triangle3D>();
            for (int i = 0; i < 3200; i++)
            {
                triangles.Add(new Triangle3D(v[f[i + 1].getX()], v[f[i + 1].getY()], v[f[i + 1].getZ()]));
            }
            m = new Mesh(v, triangles, mat);
            m.Translate(loc);
            return m;
        }*/


    }
}