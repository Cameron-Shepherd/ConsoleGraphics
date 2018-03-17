using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGraphics.Configuration
{
    static class ByteToAsciiConfiguration
    {
        public static char[] GetCharArray()
        {
            var gradient = new char[255];
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ConsoleGraphics.External.gradient.txt";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var sr = new StreamReader(stream))
            {
                string line;
                for (int i = 0; i < byte.MaxValue; i++)
                {
                    line = sr.ReadLine();
                    gradient[i] = line[0];
                }
            }

            return gradient;
        }
    }
}
