using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Drawing;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.Display
{
    public class RectangleSelection : SelectStrategy
    {
        public override Rectangle CalRect(Point corner1, Point corner2)
        {
            int xDis = Math.Abs(corner1.X - corner2.X);
            int yDis = Math.Abs(corner1.Y - corner2.Y);
            Rectangle rect = new Rectangle(Math.Min(corner1.X, corner2.X), Math.Min(corner1.Y, corner2.Y), xDis, yDis);
            return rect;
        }
    }
}
