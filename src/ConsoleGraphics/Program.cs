using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using ConsoleGraphics.Core;
using ConsoleGraphics.Core.Light;

namespace ConsoleGraphics
{
    public class Program
    {

        static Tuple<int, ConsoleColor>[,] pixels;
        static int origWidth;
        static int origHeight;
        static char[] gradient = new char[256];
        static Camera camera;
        static Scene scene;
        static bool shaded = true;
        static void Main(string[] args)
        {
            Point2D test = new Point3D(1, 1, 1);

            Console.WindowHeight = 40;
            Console.WindowWidth = 120;
            origWidth = Console.WindowWidth;
            origHeight = Console.WindowHeight;
            pixels = new Tuple<int, ConsoleColor>[origWidth, origHeight];
            ReadGradient();

            camera = new Camera(new Point3D(0, 0, 50), new Point3D(0, 0, -1), new Point3D(0, 1, 0), 20, 3000, 60);
            scene = new Scene(camera);

            Light lightscr = new Light(new Point3D(500, 500, 0), .9f);
            scene.Add(lightscr);
            lightscr = new Light(new Point3D(-100, -100, 200), 1f);
            scene.Add(lightscr);

            List<string> scenes = new List<string>() { "Pyramid", "Cube", "Sphere", "Cylinder", "Regular Guy", "Tie Fighter", " ","Generate Scene" };
            int currSelect = 0;

            Mesh tieFighter;
            Mesh Pyramid;
            Mesh Cube;
            Mesh Sphere;
            Mesh Cylinder;
            Mesh RegularGuy = MeshReader.textConvert("MaleLow.txt", new Point3D(100, 200, -400), new Material(ConsoleColor.Green));

            ConsoleKeyInfo cki;
            Boolean exit = true;
            int itemCount = 0;
            do
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Please select a scene to render using the UP/DOWN Arrows to navigate");
                Console.WriteLine("press ENTER to select the object");
                Console.WriteLine("to load scene select generate scene\n");
                foreach (string s in scenes)
                {
                    Console.Write("\t\t");
                    if (scenes[currSelect] == s) // current selection
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    Console.WriteLine(s);
                    Console.BackgroundColor = ConsoleColor.Black;
                }

                Console.WriteLine("\n\tItems In Scene: " + itemCount);
                Console.WriteLine("\n\n---Scene Camera Controls---");
                Console.WriteLine(" W-MOVE IN \n A-MOVE LEFT \n S-MOVE AWAY \n D-MOVE RIGHT \n SPACE-MOVE UP \n V-MOVE DOWN \n Q-PAN LEFT \n E-PAN RIGHT\n R-RESET CAMERA \n\n (NOT RECOMMENDED)\n UPARROW-TILT UP \n DOWN ARROW-TILT DOWN");
                Console.WriteLine("\n---Scene Physical Controls---\n G-TOGGLE GRAVITY \n H-TOGGLE NEGATIVE GRAVITY \n O-STOP ALL MOTION \n L-SPIN ALL OBJECTS \n P-TOGGLE SHADING");
                cki = Console.ReadKey();
                switch (cki.Key)
                {
                    case ConsoleKey.DownArrow:
                        currSelect += 1;
                        currSelect = Math.Min(scenes.Count - 1, currSelect);
                        break;
                    case ConsoleKey.UpArrow:
                        currSelect -= 1;
                        currSelect = Math.Max(0, currSelect);
                        break;
                    case ConsoleKey.Enter:
                        itemCount++;
                        Point3D randLoc;
                        Random rand = new Random();
                        float z = rand.Next(-800, -400);
                        float y = rand.Next(-200, 200);
                        float x = rand.Next(-200, 200);
                        ConsoleColor colour = (ConsoleColor)rand.Next(0, 16);
                        float spinX = rand.Next(-500, 500);
                        spinX = spinX / 10000;
                        float spinY = rand.Next(-500, 500);
                        spinY = spinY / 10000;
                        float spinZ = rand.Next(-500, 500);
                        spinZ = spinZ / 10000;
                        randLoc = new Point3D(x, y, z);
                        switch (currSelect)
                        {
                            case 0:
                                PhysicsObject pyr = new PhysicsObject(Mesh.GeneratePyramid(100, 100, 100, randLoc, new Material(colour)), 1);
                                scene.Add(pyr);
                                pyr.Velocity = randLoc / 100.0f;
                                pyr.AngularVelocity = new Point3D(spinX, spinY, spinZ);
                                break;
                            case 1:
                                PhysicsObject phyCube = new PhysicsObject(Mesh.GenerateRectangle(100, 100, 100, randLoc, new Material(colour)), 1);
                                scene.Add(phyCube);
                                phyCube.Velocity = randLoc / 100.0f;
                                phyCube.AngularVelocity = new Point3D(spinX, spinY, spinZ);
                                break;
                            case 2:
                                PhysicsObject phySphere = new PhysicsObject(Mesh.GenerateSphere(50, randLoc, new Material(colour)),1);
                                scene.Add(phySphere);
                                phySphere.Velocity = randLoc / 100.0f;
                                phySphere.AngularVelocity = new Point3D(spinX, spinY, spinZ);
                                break;
                            case 3:
                                PhysicsObject phyCylinder = new PhysicsObject(Mesh.GenerateCylinder(100, 50, randLoc, new Material(colour)),1);
                                scene.Add(phyCylinder);
                                phyCylinder.Velocity = randLoc / 100.0f;
                                phyCylinder.AngularVelocity = new Point3D(spinX, spinY, spinZ);
                                break;
                            case 4:
                                PhysicsObject phyRegularGuy = new PhysicsObject ( MeshReader.textConvert("MaleLow.txt", randLoc, new Material(colour)),1);
                                scene.Add(phyRegularGuy);
                                phyRegularGuy.getMesh().Scale(10);
                                phyRegularGuy.Velocity = randLoc / 100.0f;
                                phyRegularGuy.AngularVelocity = new Point3D(spinX, spinY, spinZ);
                                break;
                            case 5:
                                PhysicsObject phytieFighter = new PhysicsObject (MeshReader.textConvert("TIE-fighterLow.txt", randLoc, new Material(colour)),1);
                                scene.Add(phytieFighter);
                                phytieFighter.Velocity = randLoc/100.0f;
                                phytieFighter.AngularVelocity = new Point3D(spinX, spinY, spinZ);
                                break;
                            case 6:
                                itemCount--;
                                break;
                            
                        }
                        //if (currSelect != scenes.Count - 1)
                          //  currSelect = 0;
                        break;
                }
                
            }
            while ((cki.Key != ConsoleKey.Enter || currSelect != scenes.Count - 1) && exit);

            Thread cin = new Thread(getInput);
            cin.Start();

            //lightscr = new Light(new Point3D(-550, -450, -400), 1f);
            // scene.Add(lightscr);

           /* PhysicsObject m = new PhysicsObject(Mesh.GeneratePyramid(100, 100, 100, new Point3D(200, 17, -300), new Material(ConsoleColor.Red)), 1);
           // scene.Add(m);
            m.AngularVelocity = new Point3D(0.0f, 0f, 0.0f);
            m.Velocity = new Point3D(0.0f, 0.5f, 0.0f);
            PhysicsObject n = new PhysicsObject(Mesh.GeneratePyramid(100, 100, 100, new Point3D(200, 300, -300), new Material(ConsoleColor.Blue)), 2);
           // scene.Add(n);
            n.AngularVelocity = new Point3D(0.0f, 0f, 0.0f);
            n.Velocity = new Point3D(-0.0f, -0.5f, 0.0f);
            PhysicsObject q = new PhysicsObject(Mesh.GeneratePyramid(50, 100, 75, new Point3D(400, 200, -500), new Material(ConsoleColor.Cyan)), 1);
            //scene.Add(q);
            PhysicsObject v = new PhysicsObject(Mesh.GeneratePyramid(20, 150, 70, new Point3D(200, 400, -200), new Material(ConsoleColor.Green)), 1);
            //scene.Add(v);

            PhysicsObject circle1 = new PhysicsObject(Mesh.GenerateSphere(100, new Point3D(400, 200, -400), new Material(ConsoleColor.Yellow)), 1);
           // scene.Add(circle1);
            PhysicsObject circle2 = new PhysicsObject(Mesh.GenerateSphere(100, new Point3D(700, 200, -400), new Material(ConsoleColor.Green)), 1);
           // scene.Add(circle2);
            PhysicsObject circle3 = new PhysicsObject(Mesh.GenerateSphere(100, new Point3D(1000, 200, -400), new Material(ConsoleColor.DarkCyan)), 1);
            //scene.Add(circle3);
           // circle3.AngularVelocity = new Point3D(0.2f, 0f, 0.0f);
            //circle3.Velocity = new Point3D(0.0f, 0.5f, 0.0f);
            PhysicsObject cube = new PhysicsObject(Mesh.GenerateRectangle(100, 100, 100, new Point3D(100, 200, -400), new Material(ConsoleColor.DarkGreen)), 1);
            //scene.Add(cube);
            PhysicsObject cylinder = new PhysicsObject(Mesh.GenerateCylinder(100, 50, new Point3D(100, 00, -400), new Material(ConsoleColor.DarkCyan)), 1);
            //scene.Add(cylinder);
            //cylinder.Rotate(new Point3D((float)Math.PI / 2, 0, 0));

            PhysicsObject circle = new PhysicsObject(Mesh.GenerateSphere(100, new Point3D(100, 200, -400), new Material(ConsoleColor.Red)), 1);
            // scene.Add(circle);

            //PhysicsObject tieFighter = new PhysicsObject(MeshReader.textConvert("TIE-fighterLow.txt", new Point3D(100, 200, -400), new Material(ConsoleColor.Yellow)), 1);
            //scene.Add(tieFighter);
            */
            while (true)
            {
                foreach (PhysicsObject ob in scene.getObjects())
                {
                    ob.applyVelocity();
                    ob.applyGravity();
                }
                List<PhysicsObject> colliders = scene.collisionCheck();
                if (colliders != null)
                    PhysicsObject.interact(ref colliders);

                // reset the board
                for (int i = origHeight - 1; i >= 0; i--)
                {
                    for (int j = 0; j < origWidth; j++)
                    {
                        pixels[j, i] = new Tuple<int, ConsoleColor>(0, ConsoleColor.White);
                    }
                }

                //check if a resize occured
                if (origWidth != Console.WindowWidth || origHeight != Console.WindowHeight)
                {
                    origWidth = Console.WindowWidth;
                    origHeight = Console.WindowHeight;
                    pixels = new Tuple<int, ConsoleColor>[origWidth, origHeight];
                }

                //   m.Rotate(new Point3D(0.0f, 0.01f, 0));
                //   n.Rotate(new Point3D(0.05f, -0.01f, 0.0f));
                //   q.Rotate(new Point3D(-0.05f, -0.01f, 0.0f));
                //   v.Rotate(new Point3D(0.05f, 0.01f, 0.0f));

                //   circle1.Rotate(new Point3D(0.01f, 0.0f, 0.0f));
                //   circle2.Rotate(new Point3D(0.0f, 0.0f, 0.01f));
                //   circle3.Rotate(new Point3D(0.0f, 0.01f, 0.01f));
                //   tieFighter.Rotate(new Point3D(0.02f, 0.02f, 0.0f));

                //lightscr.RotateAround(new Point3D(150, 150, -400), new Point3D(0.05f, 0, 0));

                if (shaded)
                {
                    scene.Render(pixels);
                }
                else
                {
                    scene.RenderShaded(pixels);
                }
                draw();
            }
        }
        static void getInput()
        {
            ConsoleKeyInfo cki;

            do
            {
                cki = Console.ReadKey();
                switch (cki.Key)
                {
                    case ConsoleKey.W:
                        camera.Translate(camera.ViewVector.Normalize() * 10);
                        break;
                    case ConsoleKey.A:
                        camera.Translate(Point3D.Cross(camera.ViewVector, camera.UpVector).Normalize() * 10);
                        break;
                    case ConsoleKey.S:
                        camera.Translate(-camera.ViewVector.Normalize() * 10);
                        break;
                    case ConsoleKey.D:
                        camera.Translate(Point3D.Cross(camera.ViewVector, camera.UpVector).Normalize() * -10);
                        break;
                    case ConsoleKey.Spacebar:
                        camera.Translate(new Point3D(0, 5, 0));
                        break;
                    case ConsoleKey.V:
                        camera.Translate(new Point3D(0, -5, 0));
                        break;
                    case ConsoleKey.Q:
                        camera.Rotate(new Point3D(0, 0.05f, 0));
                        break;
                    case ConsoleKey.E:
                        camera.Rotate(new Point3D(0, -0.05f, 0));
                        break;
                    case ConsoleKey.UpArrow:
                        camera.Rotate(new Point3D(-0.05f, 0, 0));
                        break;
                    case ConsoleKey.DownArrow:
                        camera.Rotate(new Point3D(0.05f, 0, 0));
                        break;
                    case ConsoleKey.P:
                        shaded = !shaded;
                        break;
                    case ConsoleKey.R:
                        camera.Position = new Point3D(0,0,0);
                        camera.ViewVector = new Point3D(0, 0, -1);
                        camera.UpVector = new Point3D(0, 1, 0);
                        camera.calcWorldtoCameraMatrix();
                        break;
                    case ConsoleKey.G:
                        if (PhysicsObject.gravity.Y != PhysicsObject.grav.Y)
                            PhysicsObject.gravity = PhysicsObject.grav;
                        else
                            PhysicsObject.gravity = 0 * PhysicsObject.grav;
                        break;
                    case ConsoleKey.H:
                        if (PhysicsObject.gravity.Y != -PhysicsObject.grav.Y)
                            PhysicsObject.gravity = -PhysicsObject.grav;
                        else
                            PhysicsObject.gravity = 0 * PhysicsObject.grav;
                        break;
                    case ConsoleKey.O:
                        foreach (PhysicsObject ob in scene.getObjects())
                        {
                            ob.Velocity *= 0.0f;
                            ob.AngularVelocity *= 0.0f;
                        }
                        PhysicsObject.gravity *= 0.0f;
                        break;
                    case ConsoleKey.L:
                        Random rand = new Random();
                        foreach (PhysicsObject ob in scene.getObjects())
                            ob.AngularVelocity = new Point3D(rand.Next(-1,2) * 0.05f, rand.Next(-1, 2) * 0.05f, rand.Next(-1, 2) * 0.05f);
                        break;
                }

            } while (cki.Key != ConsoleKey.Escape);

        }
        private static List<Triangle3D> getAllFaces(List<Mesh> meshes)
        {
            List<Triangle3D> temp = new List<Triangle3D>();
            foreach (Mesh m in meshes)
            {
                foreach (Triangle3D t in m.Faces())
                {
                    temp.Add(t);
                }
            }
            return temp;
        }
        private static void draw()
        {
            Console.SetCursorPosition(0, 0);
            string temp = "";
            ConsoleColor curr = ConsoleColor.White;
            for (int i = 0; i < origHeight; i++)
            {
                for (int j = 0; j < origWidth; j++)
                {
                    if (pixels[j, i] != null)
                    {
                        if(pixels[j, i].Item2 != curr) // not the same color
                        {
                            Console.ForegroundColor = curr;
                            Console.Write(temp);
                            temp = "";
                            curr = pixels[j, i].Item2;
                            Console.ForegroundColor = curr;
                        }
                        temp += gradient[pixels[j, i].Item1];
                    }
                }
            }
            Console.Write(temp);
            System.Console.SetCursorPosition(0, 0);
        }
        private static void ReadGradient()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Graphic_Project3490.gradient.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader sr = new StreamReader(stream))
            {
                string line;
                for (int i = 0; i < 256; i++)
                {
                    line = sr.ReadLine();
                    gradient[i] = line[0];
                }
            }
        }
    }
}
