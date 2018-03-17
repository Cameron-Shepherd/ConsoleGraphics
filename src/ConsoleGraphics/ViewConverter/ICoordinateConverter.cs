using System.Collections.Generic;

namespace ConsoleGraphics.ViewConverter
{
    public interface ICoordinateConverter
    {
        dynamic ConvertCoordinates(IApplyMatrix item);
        IEnumerable<dynamic> ConvertListCoordinates(IEnumerable<IApplyMatrix> items);
    }
}