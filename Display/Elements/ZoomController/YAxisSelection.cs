using System;
using System.Drawing;

namespace Utilities.Display
{
    public class YAxisSelection : SelectStrategy
    {
        public override Rectangle CalRect(Point centerPoint, Point CornerPoint)
        {
            var left = 0;
            var right = Mapper.GetScreenX(ReferencSystem.Right);
            var top = Math.Min(centerPoint.Y, CornerPoint.Y);
            var bottom = Math.Max(centerPoint.Y, CornerPoint.Y);
            return new Rectangle(left, top, (int)right - left, bottom - top);
        }
    }
}
