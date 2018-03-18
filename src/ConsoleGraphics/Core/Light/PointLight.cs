namespace ConsoleGraphics.Core.Light
{
    public class PointLight : Light, IHaveLocation
    {
        public Point4D Location { get; }

        public PointLight(Point4D location, float intensity, Rgba colourRgba) : base(intensity, colourRgba)
        {
            Location = location;
        }
    }
}