using System;
using System.Drawing;

namespace Utilities.Display
{
    public class XAxisSelection : SelectStrategy
    {
        public override Rectangle CalRect(Point centerPoint, Point CornerPoint)
        {
            var left = Math.Min(centerPoint.X, CornerPoint.X);
            var right = Math.Max(centerPoint.X, CornerPoint.X);
            var top = 0;
            var bottom = Mapper.GetScreenY(ReferencSystem.Bottom);
            return new Rectangle(left, top, right - left, (int)bottom - top);
        }
    }
}
