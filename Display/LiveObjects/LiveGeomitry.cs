using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Drawing;

namespace Utilities.Display
{
    public class LiveGeometry : LiveObject
    {
        public Geometry Geometry { get; set; }
        public override bool IsPointNear(PointF mouseLocation) => Geometry.FillContainsPoint(mouseLocation.ToPoint2F());
    }
}
