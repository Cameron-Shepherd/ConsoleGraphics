namespace ConsoleGraphics.Display
{
    public abstract class Pixel
    {
        public abstract byte Value { get; }

        protected abstract byte ConvertToType();
        public static explicit operator byte(Pixel pixel)
        {
            return pixel.ConvertToType();
        }
    }
}
