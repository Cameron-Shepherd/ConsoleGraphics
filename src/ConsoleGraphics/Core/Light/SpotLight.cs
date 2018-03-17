namespace ConsoleGraphics.Core.Light
{
    public class SpotLight : Light
    {
        public Point3D Location { get; }
        public Vector Direction { get; }
        public float ConeAngle { get; }

        public SpotLight(Point3D location, Vector direction, float coneAngle, float intensity) : base(intensity)
        {
            Location = location;
            Direction = direction;
            ConeAngle = coneAngle;
        }

        public static explicit operator PointLight(SpotLight sourceLight) // explicit byte to digit conversion operator
        {
            return new PointLight(sourceLight.Location, sourceLight.Intensity);
        }
    }
}