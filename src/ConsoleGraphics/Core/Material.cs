using System;

namespace ConsoleGraphics.Core
{
    public class Material
    {
        private readonly Rgba _colour;
        public float R => _colour.Red;
        public float G => _colour.Green;
        public float B => _colour.Blue;
        public float Reflectivity { get; }
        public float Transparency => _colour.Alpha;

        public Material(Rgba colour, float reflectivity)
        {
            _colour = colour;
            Reflectivity = reflectivity;
        }
    }
}
