using System.Drawing;

namespace Utilities.Display
{
    public class Rect : MouseSensitiveObject
    {
        public Rect(RectangleF rectangle)
        {
            Rectangle = rectangle;
        }

        public RectangleF Rectangle { get; set; }
        public override bool IsMouseNear(PointF mouseLocation) => Rectangle.IsPointInRect(mouseLocation);
    }
}
