using System.Collections.Generic;
using System.Linq;
using ConsoleGraphics.Core;

namespace ConsoleGraphics.ViewConverter
{
    // if camera.position changes, create new IworldToCamerCoordinateConverter
    // use events, onChangedProperty Event
    public class WorldToCameraCoordinateConverter : IWorldToCameraCoordinateConverter
    {
        private readonly Matrix _conversionMatrix;

        public WorldToCameraCoordinateConverter(Camera camera)
        {
            var direction = camera.ViewVector.Normalize();
            var up = camera.UpVector.Normalize();
            var r = -(direction ^ up);
            var u = r ^ direction;
            
            float[,] vals = 
                {{r.X, u.X, -direction.X, 0}, {r.Y, u.Y, -direction.Y, 0}, {r.Z, u.Z, -direction.Z, 0}, {0, 0, 0, 1}};

            var mR = new Matrix(vals, 4, 4);

            Matrix mT = Matrix.Translation(-camera.Position.X, -camera.Position.Y, -camera.Position.Z);
            var mat = Matrix.Multiply(mR, mT);

            _conversionMatrix = Matrix.Transpose(mat);
        }

        public dynamic ConvertToCameraView(IApplyMatrix item)
        {
            return item.Apply(_conversionMatrix);
        }

        public IEnumerable<dynamic> ConvertListToCameraView(IEnumerable<IApplyMatrix> items)
        {
            return items.Select(x => x.Apply(_conversionMatrix));
        }
    }
}
