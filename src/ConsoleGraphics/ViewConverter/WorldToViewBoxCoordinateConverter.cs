using System.Collections.Generic;
using System.Linq;

namespace ConsoleGraphics.ViewConverter
{
    public class WorldToViewBoxCoordinateConverter : ICoordinateConverter
    {
        private readonly IWorldToCameraCoordinateConverter _worldToCamera;
        private readonly ICameraToViewBoxCoordinateConverter _cameraToViewBox;

        public WorldToViewBoxCoordinateConverter(IWorldToCameraCoordinateConverter worldToCamera, ICameraToViewBoxCoordinateConverter cameraToViewBox)
        {
            _worldToCamera = worldToCamera;
            _cameraToViewBox = cameraToViewBox;
        }

        public dynamic ConvertCoordinates(IApplyMatrix item)
        {
            var cameraView = _worldToCamera.ConvertToCameraView(item);
            var viewBoxView = _cameraToViewBox.ConvertListToViewBox(cameraView);
            return viewBoxView;
        }

        public IEnumerable<dynamic> ConvertListCoordinates(IEnumerable<IApplyMatrix> items)
        {
            var cameraView = _worldToCamera.ConvertListToCameraView(items);
            var viewBoxView = _cameraToViewBox.ConvertListToViewBox(cameraView.Cast<IApplyMatrix>());
            return viewBoxView;
        }
    }
}