using System;
using System.Drawing;

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
        private Brush fillBrush = new SolidBrush(Color.FromArgb(40, Color.Blue));
        private Rectangle coverRect;
        private RectCalculator rectCal;

        public ZoomController(RectSelectType selectType = RectSelectType.Rectangle) => rectCal = RectCalculator.Instance(selectType);

        private Pen framePen = new Pen(Color.White, 1);
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
            coverRect = rectCal.CalRect(mouseDownPos, mouseCurrentPos);
            Changed = true;
        }

        private void PictureBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            mouseDown = true;
            mouseDownPos = e.Location;
        }

        public override void Draw(Graphics g)
        {
            if (!mouseDown)
                return;
            g.FillRectangle(fillBrush, coverRect);
            g.DrawRectangle(framePen, coverRect);
            base.Draw(g);
        }

        public void Reset() => SetMapperRange(displayer.Background.XLeft, displayer.Background.XRight, displayer.Background.YTop, displayer.Background.YBottom);
    }

    public abstract class RectCalculator
    {
        public abstract Rectangle CalRect(Point centerPoint, Point CornerPoint);
        public static RectCalculator Instance(RectSelectType type)
        {
            switch (type)
            {
                case RectSelectType.Rectangle:
                    return new RectangleCalculator();
                case RectSelectType.Square:
                    return new SquareCalculator();
                default:
                    throw new Exception("错误的RectSelectType类型");
            } 
        }
    }

    internal class RectangleCalculator : RectCalculator
    {
        public override Rectangle CalRect(Point corner1, Point corner2)
        {
            int xDis = Math.Abs(corner1.X - corner2.X);
            int yDis = Math.Abs(corner1.Y - corner2.Y);
            Rectangle rect = new Rectangle(Math.Min(corner1.X, corner2.X), Math.Min(corner1.Y, corner2.Y), xDis, yDis);
            return rect;
        }
    }

    internal class SquareCalculator : RectCalculator
    {
        public override Rectangle CalRect(Point centerPoint, Point cornerPoint)
        {
            var raduas = DistanceBetween(centerPoint, cornerPoint);
            Rectangle rect = new Rectangle(centerPoint.X - (int)raduas, centerPoint.Y - (int)raduas, (int)raduas * 2, (int)raduas * 2);
            return rect;
        }

        private double DistanceBetween(Point p1, Point p2) => Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
    }
}
