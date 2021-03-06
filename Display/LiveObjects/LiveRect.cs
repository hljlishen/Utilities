﻿using System.Drawing;

namespace Utilities.Display
{
    public class LiveRect : LiveObject
    {
        public LiveRect(RectangleF rectangle)
        {
            Rectangle = rectangle;
        }

        public RectangleF Rectangle { get; set; }
        public override bool IsPointNear(PointF mouseLocation) => Rectangle.IsPointInRect(mouseLocation);
    }
}
