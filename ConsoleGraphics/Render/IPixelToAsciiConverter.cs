using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ConsoleGraphics.Configuration;
using ConsoleGraphics.Display;

namespace ConsoleGraphics.Render
{
    public interface IPixelToAsciiConverter
    {
        char GetChar(Pixel pixel);
    }

    public class PixelToAsciiConverter : IPixelToAsciiConverter
    {
        private readonly char[] _chars;

        public PixelToAsciiConverter()
        {
            _chars = ByteToAsciiConfiguration.GetCharArray();
        }

        public char GetChar(Pixel pixel)
        {
            return _chars[pixel.Value];
        }
    }
}
