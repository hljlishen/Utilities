using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
using Utilities.Coordinates;

namespace System.Drawing
{
    static class DrawingExtionFuncs
    {
        public static Point2F ToPoint2F(this PointF p) => new Point2F(p.X, p.Y);

        public static RectF ToRectF(this Rectangle r) => new RectF(r.Left, r.Top, r.Right, r.Bottom);

        public static RectF ToRectF(this RectangleF r) => new RectF(r.Left, r.Top, r.Right, r.Bottom);

        public static ColorF ToColorF(this Color c) => new ColorF(c.R / 255.0f, c.G / 255.0f, c.B / 255.0f);

        public static Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush SolidBrush(this Color c, RenderTarget rt) => rt.CreateSolidColorBrush(c.ToColorF());
        public static Point2F Center(this RectF r) => new Point2F((r.Left + r.Right) / 2, (r.Top + r.Bottom) / 2);

        public static bool IsPointInRect(this RectangleF r, PointF p) => p.X >= r.Left && p.X <= r.Right && p.Y >= r.Top && p.Y <= r.Bottom;

        public static RectangularCoordinate ToRectangularCoordinate(this PointF p) => new RectangularCoordinate(p.X, p.Y, 0);

        public static PointF ToPointF(this RectangularCoordinate r) => new PointF((float)r.X, (float)r.Y);
    }
}

namespace System
{
    public static class StringExt
    { 
        public static TextFormat MakeFormat(this string fontName, float size)
        {
            using(var dw = DWriteFactory.CreateFactory())
            {
                return dw.CreateTextFormat(fontName, size);
            }
        }
    }
}

