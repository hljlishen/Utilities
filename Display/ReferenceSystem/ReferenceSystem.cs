using System.Drawing;
using Utilities.Mapper;

namespace Utilities.Display
{
    public class ReferenceSystem
    {
        public ReferenceSystem(double left, double right, double top, double bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        public PointF ScreenOriginalPoint => Mapper.GetScreenLocation(0, 0);
        public double Left { get; private set; }
        public double Right { get; private set; }
        public double Top { get; private set; }
        public double Bottom { get; private set; }
        public IScreenToCoordinateMapper Mapper { get; private set; }
        public void SetMapper(IScreenToCoordinateMapper mapper)
        {
            Mapper = mapper;
            Mapper.SetCoordinateArea(Left, Right, Top, Bottom);
        }
        public void SetArea(double left, double right, double top, double bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
            Mapper.SetCoordinateArea(left, right, top, bottom);
        }
    }
}
