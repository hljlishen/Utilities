using System.Threading;
using System.Threading.Tasks;
using Utilities.Mapper;

namespace Utilities.Display
{
    class ZoomAnimation 
    {
        private Area targetArea;
        private IScreenToCoordinateMapper mapper;
        private int animationTime = 500;
        private int updateInterval = 30;
        private int frames;
        private double xLeftStep, xRightStep;
        private double yTopStep, yBottomStep;

        public ZoomAnimation(Area targetArea, IScreenToCoordinateMapper mapper)
        {
            this.targetArea = targetArea;
            this.mapper = mapper;
            frames = animationTime / updateInterval;
            xLeftStep = (targetArea.Left - mapper.CoordinateLeft) / frames;
            xRightStep = (targetArea.Right - mapper.CoordinateRight) / frames;
            yTopStep = (targetArea.Top - mapper.CoordinateTop) / frames;
            yBottomStep = (targetArea.Bottom - mapper.CoordinateBottom) / frames;
        }

        public void StartZoom() => Task.Run(AnimateZoom);

        private void AnimateZoom()
        {
            for(int i = 0; i < frames - 1; i++)
            {
                //Area area = new Area() { Left = mapper.ScreenLeft + xLeftStep, Right = mapper.ScreenRight + xRightStep, Top = mapper.ScreenTop + yTopStep, Bottom = mapper.ScreenBottom + yBottomStep };
                mapper.SetCoordinateArea(mapper.CoordinateLeft + xLeftStep, mapper.CoordinateRight + xRightStep, mapper.CoordinateTop + yTopStep, mapper.CoordinateBottom + yBottomStep);
                Thread.Sleep(updateInterval);
            }

            mapper.SetCoordinateArea(targetArea.Left, targetArea.Right, targetArea.Top, targetArea.Bottom); //最后一次设置映射区域，直接设置为目标区域
        }
    }
}
