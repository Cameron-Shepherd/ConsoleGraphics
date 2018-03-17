using ConsoleGraphics.Display;

namespace ConsoleGraphics.Render
{
    public class ConsoleFrameDrawer : IFrameDrawer
    {
        private readonly IStreamWriter _writer;

        public ConsoleFrameDrawer(IStreamWriter writer)
        {
            _writer = writer;
        }

        public void DrawFrame(Frame frame)
        {
            string str = "";
            //...

            _writer.Write(str);
        }
    }
}