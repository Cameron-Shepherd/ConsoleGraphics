using ConsoleGraphics.Render;

namespace ConsoleGraphics.Core.Light
{
    public abstract class Light
    {
        protected Light(float intensity)
        {
            Intensity = intensity;
        }

        public float Intensity { get; } // between 0 and 1
    }
}
