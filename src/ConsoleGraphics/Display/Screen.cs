namespace ConsoleGraphics.Display
{
    public class Screen
    {
        public int Width { get; }
        public int Height { get; }

        public Screen(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public Screen Resize(int newWidth, int newHeight)
        {
            return null;
        }
    }
}
