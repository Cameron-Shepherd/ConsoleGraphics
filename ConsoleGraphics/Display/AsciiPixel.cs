namespace ConsoleGraphics.Display
{
    public class AsciiPixel : Pixel
    {
        public override byte Value { get; }

        public AsciiPixel(byte value)
        {
            Value = value;
        }

        protected override byte ConvertToType()
        {
            return Value;
        }
    }
}
