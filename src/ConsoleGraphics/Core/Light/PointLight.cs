namespace ConsoleGraphics.Core.Light
{
    public class PointLight : Light
    {
        public Point3D Location { get; }

        public PointLight(Point3D location, float intensity) : base(intensity)
        {
            Location = location;
        }
    }
}