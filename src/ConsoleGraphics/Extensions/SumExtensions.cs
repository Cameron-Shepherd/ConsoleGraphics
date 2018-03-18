using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleGraphics.Core;

namespace ConsoleGraphics.Extensions
{
    public static class SumExtensions
    {
        public static Rgba Sum(this IEnumerable<Rgba> source)
        {
            return source.Aggregate((x, y) => x + y);
        }

        public static Rgba Sum<T>(this IEnumerable<T> source, Func<T, Rgba> selector)
        {
            return source.Select(selector).Aggregate((x, y) => x + y);
        }
    }
}
