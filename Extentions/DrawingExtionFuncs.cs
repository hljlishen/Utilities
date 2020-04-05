﻿using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;

namespace System.Drawing
{
    static class DrawingExtionFuncs
    {
        public static Point2F ToPoint2F(this PointF p) => new Point2F(p.X, p.Y);

        public static RectF ToRectF(this Rectangle r) => new RectF(r.Left, r.Top, r.Right, r.Bottom);

        public static RectF ToRectF(this RectangleF r) => new RectF(r.Left, r.Top, r.Right, r.Bottom);

        public static ColorF ToColorF(this Color c) => new ColorF(c.R, c.G, c.B);

        public static Point2F Center(this RectF r) => new Point2F((r.Left + r.Right) / 2, (r.Top + r.Bottom) / 2);
    }
}