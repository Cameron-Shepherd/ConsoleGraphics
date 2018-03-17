using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ConsoleGraphics.Core;

namespace ConsoleGraphics
{

    public class MeshReader
    {
        public static Mesh textConvert(string textFile, Point3D loc, Material mat)
        {

            List<Point3D> vPoints = new List<Point3D>();
            List<Point3D> vnPoints = new List<Point3D>();
            //List<Point3D> fRelations = new List<Point3D>();
            List<Triangle3D> faces = new List<Triangle3D>();

            // Read the file and display it line by line.

            // This is for having the textfile as an EMBEDDED RESOURCE
            // Click properties of one of the text files, you will see its an embedded resource.
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Graphic_Project3490." + textFile;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader file = new StreamReader(stream))
            {
                string line;
                string[] delimiter = new string[] { "//" };
                while ((line = file.ReadLine()) != null)
                {
                    string[] words = line.Split(' ');
                    switch (words[0])
                    {
                        case "v":
                            vPoints.Add(new Point3D((float)(float.Parse(words[1])), (float)(float.Parse(words[2])), (float)(float.Parse(words[3]))));
                            break;

                        case "vn":
                            vnPoints.Add(new Point3D(float.Parse(words[1]), float.Parse(words[2]), float.Parse(words[3])));
                            break;

                        case "f":
                            string[] wordsTrimmed1 = words[1].Split(delimiter, StringSplitOptions.None);
                            string[] wordsTrimmed2 = words[2].Split(delimiter, StringSplitOptions.None);
                            string[] wordsTrimmed3 = words[3].Split(delimiter, StringSplitOptions.None);
                            //fRelations.Add(new Point3D(float.Parse(wordsTrimmed1[0]), float.Parse(wordsTrimmed2[0]), float.Parse(wordsTrimmed3[0])));
                            faces.Add(new Triangle3D(vPoints[Convert.ToInt32(wordsTrimmed1[0])-1], vPoints[Convert.ToInt32(wordsTrimmed2[0])-1], vPoints[Convert.ToInt32(wordsTrimmed3[0])-1]));
                            break;

                    }
                }

                // add all the vertex normals to the vertexes
                for (int i = 0; i < vPoints.Count; i++)
                {
                    vPoints[i].VN = vnPoints[i];
                }

                file.Close();
                //return Mesh.generateComplexObj(vPoints, vnPoints, fRelations, loc, mat);
                Mesh m = new Mesh(vPoints, faces, mat); // regular mesh constructor
                m.TranslateTo(loc);
                return m;
            }

        }
    }
}
