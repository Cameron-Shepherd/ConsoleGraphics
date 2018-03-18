namespace ConsoleGraphics.Core.Light
{
    public class SpotLight : Light, IHaveLocation
    {
        public Point4D Location { get; }
        public Vector Direction { get; }
        public float ConeAngle { get; }

        public SpotLight(Point4D location, Vector direction, float coneAngle, float intensity, Rgba colourRgba) : base(
            intensity, colourRgba)
        {
            Location = location;
            Direction = direction;
            ConeAngle = coneAngle;
        }

        public static explicit operator PointLight(SpotLight sourceLight) // explicit byte to digit conversion operator
        {
            return new PointLight(sourceLight.Location, sourceLight.Intensity, sourceLight.Rgba);
        }
    }
}