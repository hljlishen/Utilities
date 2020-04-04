using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Drawing;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.Display
{
    public enum RectSelectType
    {
        Rectangle,
        Square
    }
    public class ZoomController : GraphicElement
    {
        private bool mouseDown = false;
        private Point mouseDownPos;
        private Point mouseCurrentPos;
        private Brush fillBrush;
        private Brush frameBrush;
        private Rectangle coverRect;
        private SelectStrategy selectStrategy;

        public override void Dispose()
        {
            base.Dispose();
            fillBrush?.Dispose();
            frameBrush?.Dispose();
        }

        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            fillBrush = rt.CreateSolidColorBrush(Color.Blue.ToColorF());
            fillBrush.Opacity = 0.5f;
            frameBrush = rt.CreateSolidColorBrush(Color.White.ToColorF());
        }

        public ZoomController(RectSelectType selectType = RectSelectType.Rectangle) => selectStrategy = SelectStrategy.Instance(selectType);
        public override void SetDisplayer(Displayer d)
        {
            base.SetDisplayer(d);
            PictureBox.MouseDown += PictureBox_MouseDown;
            PictureBox.MouseMove += PictureBox_MouseMove;
            PictureBox.MouseUp += PictureBox_MouseUp;
        }

        private void PictureBox_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            mouseDown = false;
            SetMapperRange(Mapper.GetCoordinateX(coverRect.Left), Mapper.GetCoordinateX(coverRect.Right), Mapper.GetCoordinateY(coverRect.Top), Mapper.GetCoordinateY(coverRect.Bottom));
        }

        private void SetMapperRange(double xLeft, double xRight, double yTop, double yBottom)
        {
            Mapper.SetCoordinateXRange(xLeft, xRight);
            Mapper.SetCoordinateYRange(yTop, yBottom);
            displayer.Redraw = true;
            Changed = true;
        }

        private void PictureBox_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!mouseDown)
                return;
            mouseCurrentPos = e.Location;
            coverRect = selectStrategy.CalRect(mouseDownPos, mouseCurrentPos);
            Changed = true;
        }

        private void PictureBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            mouseDown = true;
            mouseDownPos = e.Location;
        }

        public override void Draw(RenderTarget rt)
        {
            base.Draw(rt);
            if (!mouseDown)
                return;
            selectStrategy.DrawZoomView(coverRect.ToRectF(), rt, fillBrush, frameBrush, 2);
        }

        public void Reset() => SetMapperRange(displayer.Background.XLeft, displayer.Background.XRight, displayer.Background.YTop, displayer.Background.YBottom);
    }

    public abstract class SelectStrategy
    {
        public abstract Rectangle CalRect(Point centerPoint, Point CornerPoint);
        public abstract void DrawZoomView(RectF coverRect, RenderTarget rt, Brush fillBrush, Brush frameBrush, float strokeWidth);
        public static SelectStrategy Instance(RectSelectType type)
        {
            switch (type)
            {
                case RectSelectType.Rectangle:
                    return new RectangleStrategy();
                case RectSelectType.Square:
                    return new SquareStrategy();
                default:
                    throw new Exception("错误的RectSelectType类型");
            }
        }
    }

    internal class RectangleStrategy : SelectStrategy
    {
        public override Rectangle CalRect(Point corner1, Point corner2)
        {
            int xDis = Math.Abs(corner1.X - corner2.X);
            int yDis = Math.Abs(corner1.Y - corner2.Y);
            Rectangle rect = new Rectangle(Math.Min(corner1.X, corner2.X), Math.Min(corner1.Y, corner2.Y), xDis, yDis);
            return rect;
        }

        public override void DrawZoomView(RectF coverRect, RenderTarget rt, Brush fillBrush, Brush frameBrush, float strokeWidth)
        {
            rt.FillRectangle(coverRect, fillBrush);
            rt.DrawRectangle(coverRect, frameBrush, strokeWidth);
        }
    }

    internal class SquareStrategy : SelectStrategy
    {
        public override Rectangle CalRect(Point centerPoint, Point cornerPoint)
        {
            var raduas = DistanceBetween(centerPoint, cornerPoint);
            Rectangle rect = new Rectangle(centerPoint.X - (int)raduas, centerPoint.Y - (int)raduas, (int)raduas * 2, (int)raduas * 2);
            return rect;
        }

        public override void DrawZoomView(RectF coverRect, RenderTarget rt, Brush fillBrush, Brush frameBrush, float strokeWidth)
        {
            Ellipse e = new Ellipse(coverRect.Center(), coverRect.Width / 2, coverRect.Width / 2);
            rt.FillEllipse(e, fillBrush);
            rt.DrawEllipse(e, frameBrush, strokeWidth);
        }

        private double DistanceBetween(Point p1, Point p2) => Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
    }
}
