using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleGraphics.Core;
using ConsoleGraphics.Core.Light;
using ConsoleGraphics.Extensions;

namespace ConsoleGraphics.Render
{
    public class LuminosityCalculator : ILuminosityCalculator
    {
        public Rgba Luminosity(Point4D location, Vector normal, Light sourceLight)
        {
            switch (sourceLight)
            {
                case SpotLight spotLight:
                    return sourceLight.Rgba * LuminositySpotLight(location, normal, spotLight);
                case PointLight pointLight:
                    return sourceLight.Rgba * LuminosityPointLight(location, normal, pointLight);
                case DirectionLight directionLight:
                    return sourceLight.Rgba * LuminosityDirectionLight(normal, directionLight);
                case AmbientLight ambientLight:
                    return sourceLight.Rgba * LuminosityAmbientLight(ambientLight);
            }

            throw new ArgumentException("Light Type not supported");
        }

        public Rgba Luminosity(Point4D location, Vector normal, IEnumerable<Light> sourceLight)
        {
            var light = sourceLight.Sum(x => Luminosity(location, normal, x));
            return light; //TODO: check for light values that are over??
        }

        internal byte LuminositySpotLight(Point4D location, Vector normal, SpotLight sourceLight)
        {
            var lightVector = (sourceLight.Location - location);
            var dotProduct = (sourceLight.Direction * lightVector) / (lightVector.Magnitude * sourceLight.Direction.Magnitude);

            if (Math.Acos(dotProduct) > sourceLight.ConeAngle / 2) // check if it sees object
                return 0;

            return LuminosityPointLight(location, normal, (PointLight)sourceLight);
        }

        internal byte LuminosityPointLight(Point4D location, Vector normal, PointLight sourceLight)
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