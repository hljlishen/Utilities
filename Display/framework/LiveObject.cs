﻿using System.Drawing;

namespace Utilities.Display
{
    public abstract class LiveObject
    {
        public PointF MouseLocation { get; set; }
        public object Value { get; set; }
        public bool Selected { get; set; } = false;
        public abstract bool IsPointNear(PointF mouseLocation);
    }
}
