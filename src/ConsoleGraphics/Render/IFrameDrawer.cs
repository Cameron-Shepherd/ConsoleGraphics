using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleGraphics.Core;
using ConsoleGraphics.Display;

namespace ConsoleGraphics.Render
{
    public interface IFrameDrawer
    {
        void DrawFrame(Frame frame);
    }

    class FrameDrawer : IFrameDrawer
    {
        private readonly IRender _renderer;
        private readonly IStreamWriter _writer;

        public FrameDrawer(IRender renderer, IStreamWriter writer)
        {
            _renderer = renderer;
            _writer = writer;
        }

        void DrawFrame(Scene scene, Camera camera)
        {
            var frame = _renderer.RenderFrame(scene, camera);
            _writer.Write(frame);
        }
    }
}
