using System.Collections.Generic;

namespace ConsoleGraphics.ViewConverter
{
    public interface IWorldToCameraCoordinateConverter
    {
        dynamic ConvertToCameraView(IApplyMatrix item);
        IEnumerable<dynamic> ConvertListToCameraView(IEnumerable<IApplyMatrix> items);
    }
}