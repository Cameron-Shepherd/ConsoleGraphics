using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleGraphics.Core;
using ConsoleGraphics.Core.Light;

namespace ConsoleGraphics.Render
{
    public class LuminosityCalculator : ILuminosityCalculator
    {
        public byte Luminosity(Point3D location, Vector normal, Light sourceLight)
        {
            switch (sourceLight)
            {
                case SpotLight spotLight:
                    return LuminositySpotLight(location, normal, spotLight);
                case PointLight pointLight:
                    return LuminosityPointLight(location, normal, pointLight);
                case DirectionLight directionLight:
                    return LuminosityDirectionLight(normal, directionLight);
                case AmbientLight ambientLight:
                    return LuminosityAmbientLight(ambientLight);
            }

            throw new ArgumentException("Light Type not supported");
        }

        public byte Luminosity(Point3D location, Vector normal, IEnumerable<Light> sourceLight)
        {
            var light = sourceLight.Sum(x => Luminosity(location, normal, x));
            return light > byte.MaxValue ? byte.MaxValue : (byte) light;
        }

        internal byte LuminositySpotLight(Point3D location, Vector normal, SpotLight sourceLight)
        {
            var lightVector = (sourceLight.Location - location);
            var dotProduct = (sourceLight.Direction * lightVector) / (lightVector.Magnitude * sourceLight.Direction.Magnitude);

            if (Math.Acos(dotProduct) > sourceLight.ConeAngle / 2)
                return 0;

            return LuminosityPointLight(location, normal, (PointLight)sourceLight);
        }

        internal byte LuminosityPointLight(Point3D location, Vector normal, PointLight sourceLight)
        {
            var lightVector = (location - sourceLight.Location);
            var distanceIntensity = 1.0f / lightVector.Magnitude * sourceLight.Intensity;
            var dotProduct = (normal * lightVector) / (lightVector.Magnitude * normal.Magnitude);

            if (dotProduct > 0)
                return 0;

            return (byte)(distanceIntensity * (-dotProduct) * byte.MaxValue);
        }

        internal byte LuminosityDirectionLight(Vector normal, DirectionLight sourceLight)
        {
            var dotProduct = (normal * sourceLight.Direction) / (sourceLight.Direction.Magnitude * normal.Magnitude);

            if (dotProduct > 0)
                return 0;

            return (byte)((-dotProduct) * byte.MaxValue);
        }

        internal byte LuminosityAmbientLight(AmbientLight sourceLight)
        {
            return (byte)(byte.MaxValue * sourceLight.Intensity);
        }
    }
}