using System.Collections.Generic;

namespace ConsoleGraphics.ViewConverter
{
    public interface ICameraToViewBoxCoordinateConverter
    {
        dynamic ConvertToViewBox(IApplyMatrix item);
        IEnumerable<dynamic> ConvertListToViewBox(IEnumerable<IApplyMatrix> items);
    }
}