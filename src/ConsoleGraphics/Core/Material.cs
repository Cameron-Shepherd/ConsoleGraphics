using System;

namespace ConsoleGraphics.Core
{
    public class Material
    {
        public Rgba Rgba { get; }
        public float R => Rgba.Red;
        public float G => Rgba.Green;
        public float B => Rgba.Blue;
        public float Reflectivity { get; }
        public float Transparency => Rgba.Alpha;

        public Material(Rgba colour, float reflectivity)
        {
            Rgba = colour;
            Reflectivity = reflectivity;
        }
    }
}
