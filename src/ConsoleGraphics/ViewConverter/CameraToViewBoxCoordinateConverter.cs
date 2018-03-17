using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleGraphics.Core;

namespace ConsoleGraphics.ViewConverter
{
    public class CameraToViewBoxCoordinateConverter : ICameraToViewBoxCoordinateConverter
    {
        private readonly Matrix _conversionMatrix;

        public CameraToViewBoxCoordinateConverter(float viewAngle, float nearPlaneDistance, float farPlaneDistance)
        {
            float[,] vals = 
            {
                {(1 / (float) Math.Tan(viewAngle / 2)), 0, 0, 0}, 
                {0, (1 / (float) Math.Tan(viewAngle / 2)), 0, 0},
                {0, 0, (farPlaneDistance + nearPlaneDistance) / (farPlaneDistance - nearPlaneDistance), -1},
                {0, 0, (2 * farPlaneDistance * nearPlaneDistance) / (farPlaneDistance - nearPlaneDistance), 0}
            };
            _conversionMatrix = new Matrix(vals, 4, 4);
        }

        public dynamic ConvertToViewBox(IApplyMatrix item)
        {
            return item.Apply(_conversionMatrix);
        }

        public IEnumerable<dynamic> ConvertListToViewBox(IEnumerable<IApplyMatrix> items)
        {
            return items.Select(x => x.Apply(_conversionMatrix));
        }
    }
}