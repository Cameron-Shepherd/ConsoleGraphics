using ConsoleGraphics.Render;

namespace ConsoleGraphics.Core.Light
{
    public abstract class Light
    {
        public Rgba Rgba { get; }
        public float Intensity { get; } // between 0 and 1
        public float R => Rgba.Red;
        public float G => Rgba.Green;
        public float B => Rgba.Blue;
        public float A => Rgba.Alpha;

        protected Light(float intensity, Rgba rbga)
        {
            Rgba = rbga;
            Intensity = intensity;
        }
    }

    public interface IHaveLocation
    {
        Point4D Location { get; }
    }
}
