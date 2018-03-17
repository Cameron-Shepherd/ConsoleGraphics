using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleGraphics.Core.Light;
using ConsoleGraphics.Render;

namespace ConsoleGraphics.Core
{
    public class Scene : IApplyMatrix
    {
        private List<Light.Light> lights;
        private List<Mesh> meshes;
        private List<PhysicsObject> PhysicsObjects;
        private Camera camera;
        private static char[] gradient;

        public IEnumerable<Mesh> Meshes => meshes;
        public IEnumerable<Light.Light> Lights => lights;

        public Scene()
        {
        }
        public Scene(Camera c)
        {
            this.camera = c;
            lights = new List<Light.Light>();
            meshes = new List<Mesh>();
            PhysicsObjects = new List<PhysicsObject>();
            gradient = new char[256];
            //readGradient();
        }

        public dynamic Apply(Matrix a)
        {
            throw new NotImplementedException();
        }

        public Triangle3D GetIntersectingTriangle(Ray ray)
        {
            float minZ = float.MaxValue;
            foreach (var tri in Meshes.SelectMany(x => x.Faces))
            {
                tri.Normal * (tri.V1 - tri.V2);
            }
            return null;
        }

        public void Add(Mesh m) { meshes.Add(m); }
        public void Add(PhysicsObject s) { PhysicsObjects.Add(s); meshes.Add(s.getMesh()); }
        public void Add(Light.Light l) { lights.Add(l); }

        public void RenderShaded(Tuple<int, ConsoleColor>[,] screen)
        {
            int screenWidth = screen.GetLength(0);
            int screenHeight = screen.Length / screen.GetLength(0);

            List<Tuple<Triangle3D, int, int, int, ConsoleColor>> tri = new List<Tuple<Triangle3D, int, int, int, ConsoleColor>>();
            foreach (Mesh m in meshes)
            {
                // get all the faces, in some random order
                List<Triangle3D> faces = m.Faces();
                //List<Triangle3D> temp = new List<Triangle3D>();

                foreach (Triangle3D t in faces)
                {
                    int l1 = (int)t.V1.getLightValue(lights, camera.ViewVector);
                    int l2 = (int)t.V2.getLightValue(lights, camera.ViewVector);
                    int l3 = (int)t.V3.getLightValue(lights, camera.ViewVector);

                    if (!m.Shaded)
                    {
                        l1= (int)(new Point3D(t.Center.X, t.Center.Y, t.Center.Z, t.Normal)).getLightValue(lights, camera.ViewVector);
                        l2 = l1;
                        l3 = l2;
                    }
                    //int lightVal = (int)t.getLightValue(lights);
                    //convert to camera space
                    //sort this triangle list
                    Matrix conversion = camera.WorldtoCameraMatrix;
                    Matrix viewConversion = camera.getCameraViewMatrix();

                    Point3D reference = t.V1;
                    Matrix cameraVersion = reference * conversion;
                    //Matrix cameraVersion = conversion * reference;
                    Matrix v1boxCoords = cameraVersion * viewConversion;
                    Point3D boxV1 = v1boxCoords.boxCoordsToPoint3D();

                    reference = t.V2;
                    cameraVersion = reference * conversion;
                    Matrix v2boxCoords = cameraVersion * viewConversion;
                    Point3D boxV2 = v2boxCoords.boxCoordsToPoint3D();

                    reference = t.V3;
                    cameraVersion = reference * conversion;
                    Matrix v3boxCoords = cameraVersion * viewConversion;
                    Point3D boxV3 = v3boxCoords.boxCoordsToPoint3D();
                    Triangle3D boxTriangle = new Triangle3D(boxV1, boxV2, boxV3);
                    ConsoleColor col = ConsoleColor.White;
                    if (m.Material != null)
                        col = m.Material.Colour;
                    //if (Point3D.Dot(camera.ViewVector, t.Normal) > -.5)
                    Tuple<Triangle3D, int, int, int, ConsoleColor> tuple = new Tuple<Triangle3D, int, int, int, ConsoleColor>(boxTriangle, l1, l2, l3, col);
                    tri.Add(tuple);
                }

            }
            List<Tuple<Triangle3D, int, int, int, ConsoleColor>> ordered = tri.OrderBy(o => o.Item1.Center.Z).ToList();

            foreach (Tuple<Triangle3D, int, int, int, ConsoleColor> t in ordered)
            {
                Triangle3D triTemp = t.Item1;
                if (triangleInView(triTemp))
                {
                    triTemp.Translate(new Point3D(1, 1, 0));
                    triTemp.Scale(new Point3D(screenWidth / 2, screenHeight / 2, 1));

                    Triangle2D tri2D = new Triangle2D(new Point2D(triTemp.V1.X, triTemp.V1.Y), new Point2D(triTemp.V2.X, triTemp.V2.Y), new Point2D(triTemp.V3.X, triTemp.V3.Y));

                    // screen, lightvalue, color
                    if (triangleInBounds(tri2D, screen) && tri2D.Area < 4000)
                        tri2D.drawTriangleShaded(screen, t.Item2, t.Item3, t.Item4, t.Item5);
                }
            }

        }

        public void Render(Tuple<int, ConsoleColor>[,] screen)
        {
            int screenWidth = screen.GetLength(0);
            int screenHeight = screen.Length / screen.GetLength(0);

            List<Tuple<Triangle3D, int, ConsoleColor>> orderedTriangles = getTriangles();
            foreach (Tuple<Triangle3D, int, ConsoleColor> t in orderedTriangles)
            {
                Triangle3D tri = t.Item1;
                if (triangleInView(tri))
                {
                    tri.Translate(new Point3D(1, 1, 0));
                    tri.Scale(new Point3D(screenWidth / 2, screenHeight / 2, 1));

                    Triangle2D tri2D = new Triangle2D(new Point2D(tri.V1.X, tri.V1.Y), new Point2D(tri.V2.X, tri.V2.Y), new Point2D(tri.V3.X, tri.V3.Y));

                    // screen, lightvalue, color
                    if (triangleInBounds(tri2D, screen) && tri2D.Area < 4000)
                        tri2D.drawTriangle(screen, t.Item2, t.Item3);
                }
            }

        }
        private bool triangleInView(Triangle3D tri)
        {
            return (tri.V1.Z <= 1 && tri.V1.Z >= -1) &&
                (tri.V2.Z <= 1 && tri.V2.Z >= -1) &&
                (tri.V3.Z <= 1 && tri.V3.Z >= -1) &&
                ((tri.V1.X <= 1 && tri.V1.X >= -1) ||
                (tri.V2.X <= 1 && tri.V2.X >= -1) ||
                (tri.V3.X <= 1 && tri.V3.X >= -1)) &&
                ((tri.V1.Y <= 1 && tri.V1.Y >= -1) ||
                (tri.V2.Y <= 1 && tri.V2.Y >= -1) ||
                (tri.V3.Y <= 1 && tri.V3.Y >= -1));
        }
        private bool triangleInBounds(Triangle2D tri, Tuple<int, ConsoleColor>[,] screen)
        {
            int screenWidth = screen.GetLength(0);
            int screenHeight = screen.Length / screen.GetLength(0);
            return (tri.V1.X >= 0 && tri.V1.X < screenWidth && tri.V1.Y >= 0 && tri.V1.Y < screenHeight) ||
                (tri.V2.X >= 0 && tri.V2.X < screenWidth && tri.V2.Y >= 0 && tri.V2.Y < screenHeight) ||
                (tri.V3.X >= 0 && tri.V3.X < screenWidth && tri.V3.Y >= 0 && tri.V3.Y < screenHeight);
        }
        private List<Tuple<Triangle3D, int, ConsoleColor>> getTriangles()
        {
            List<Tuple<Triangle3D, int, ConsoleColor>> tri = new List<Tuple<Triangle3D, int, ConsoleColor>>();
            foreach (Mesh m in meshes)
            {
                // get all the faces, in some random order
                List<Triangle3D> faces = m.Faces();
                //List<Triangle3D> temp = new List<Triangle3D>();
                foreach (Triangle3D t in faces)
                {
                    //int lightVal = (int)t.getLightValue(lights);
                    int lightVal = (int)(new Point3D(t.Center.X, t.Center.Y, t.Center.Z, t.Normal)).getLightValue(lights, camera.ViewVector);
                    //convert to camera space
                    //sort this triangle list
                    Matrix conversion = camera.WorldtoCameraMatrix;
                    Matrix viewConversion = camera.getCameraViewMatrix();

                    Point3D reference = t.V1;
                    Matrix cameraVersion = reference * conversion;
                    //Matrix cameraVersion = conversion * reference;
                    Matrix v1boxCoords = cameraVersion * viewConversion;
                    Point3D boxV1 = v1boxCoords.boxCoordsToPoint3D();

                    reference = t.V2;
                    cameraVersion = reference * conversion;
                    Matrix v2boxCoords = cameraVersion * viewConversion;
                    Point3D boxV2 = v2boxCoords.boxCoordsToPoint3D();

                    reference = t.V3;
                    cameraVersion = reference * conversion;
                    Matrix v3boxCoords = cameraVersion * viewConversion;
                    Point3D boxV3 = v3boxCoords.boxCoordsToPoint3D();
                    Triangle3D boxTriangle = new Triangle3D(boxV1, boxV2, boxV3);
                    ConsoleColor col = ConsoleColor.White;
                    if (m.Material != null)
                        col = m.Material.Colour;
                    //if (Point3D.Dot(camera.ViewVector, t.Normal) > -.5)
                    Tuple<Triangle3D, int, ConsoleColor> tuple = new Tuple<Triangle3D, int, ConsoleColor>(boxTriangle, lightVal, col);
                    tri.Add(tuple);
                }
            }
            List<Tuple<Triangle3D, int, ConsoleColor>> ordered = tri.OrderBy(o => o.Item1.Center.Z).ToList();
            return ordered;
        }

        public List<PhysicsObject> getObjects()
        {
            return PhysicsObjects;
        }
        public List<PhysicsObject> collisionCheck()
        {
            List<PhysicsObject> colliders = null;
            int count = PhysicsObjects.Count;
            if (count > 1)
            {
                for (int i = 0; i < count; i++)
                {
                    for (int n = i + 1; n < count; n++)
                    {
                        if ((PhysicsObjects[i].MassCenter - PhysicsObjects[n].MassCenter).Magnitude <= PhysicsObjects[i].Radius + PhysicsObjects[n].Radius)
                        {
                            colliders = new List<PhysicsObject>();
                            colliders.Add(PhysicsObjects[i]);
                            colliders.Add(PhysicsObjects[n]);
                            return colliders;
                        }
                    }//for
                }//for
            }//else
            return colliders;
        }
        /*private static void readGradient()
        {
            using (StreamReader sr = new StreamReader("gradient.txt"))
            {
                string line;
                for (int i = 0; i < 256; i++)
                {
                    line = sr.ReadLine();
                    gradient[i] = line[0];
                }
            }
        }*/

        
    }
}
