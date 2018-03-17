using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConsoleGraphics.Display;

namespace ConsoleGraphics.Render
{
    public interface IFrameBuffer
    {
    }

    public class FrameBuffer : IFrameBuffer
    {
        private readonly IFrameDrawer _frameDrawer;
        private Queue<Frame> frames;
        private DateTime lastDrawnTime;
        private double targetFramerate;

        public FrameBuffer(IFrameDrawer frameDrawer)
        {
            _frameDrawer = frameDrawer;

            //Thread newThread = new Thread(drawNextFrame);
        }
    
        //trigger method/event to check to render next frame??
        public void AddFrame(Frame frame)
        {
            frames.Enqueue(frame);
        }
    }
}
