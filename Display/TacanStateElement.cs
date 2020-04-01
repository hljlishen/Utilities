using System;
using System.Drawing;
using System.Windows.Forms;
using Utilities.Coordinates;
using Utilities.Guards;
using Utilities.Mapper;
using Utilities.Tools;

namespace Utilities.Display
{
    public class TacanStateElement : DynamicElement<PolarCoordinate>
    {
        public PointF screenLocation;
        public PointF CoordinateLocation;
        private bool mouseDown = false;
        private PolarCoordinate pLoc;
        private RectangularCoordinate rLoc;

        public TacanStateElement(PointF initLocation)
        {
            Guard.AssertNotNull(initLocation);
            screenLocation = initLocation;
            rLoc = new RectangularCoordinate(0, 0, 0);
            pLoc = new PolarCoordinate(0, 0, 0);
        }

        public void Update(PointF screenLocation, IScreenToCoordinateMapper mapper)
        {
            var rCoo = mapper.GetCoordinateLocation(screenLocation.X, screenLocation.Y);
            rLoc = new RectangularCoordinate(rCoo.X, rCoo.Y, 0);
            pLoc = rLoc.Polar;
            this.screenLocation = screenLocation;
        }

        public static RectangularCoordinate PointFToRCoordinate(PointF p) => new RectangularCoordinate(p.X, p.Y, 0);

        protected override void DoDraw(Graphics graphics)
        {
            Pen p = new Pen(Color.Red, 1f);
            var point = Mapper.GetScreenLocation(rLoc.X, rLoc.Y);
            var center = Mapper.ScreenCenter;
            var xPoint = new PointF(point.X, center.Y);
            var yPoint = new PointF(center.X, point.Y);
            var xmPoint = new PointF(point.X, center.Y - (center.Y - point.Y) / 2);
            var ymPoint = new PointF(center.X + (point.X - center.X) / 2, point.Y);
            var dmPoint = new PointF(center.X + (point.X - center.X) / 2, center.Y - (center.Y - point.Y) / 2);
            graphics.DrawLine(p, point, center);
            graphics.DrawLine(p, point, xPoint);
            graphics.DrawLine(p, point, yPoint);
            Font font = new Font("宋体", 12);
            SolidBrush brush = new SolidBrush(Color.Yellow);
            graphics.DrawString(rLoc.X.ToString("0"), font, brush, ymPoint);
            graphics.DrawString((rLoc.Y).ToString("0"), font, brush, xmPoint);
            graphics.DrawString(pLoc.Dis.ToString("0"), font, brush, dmPoint);
            graphics.FillEllipse(new SolidBrush(Color.Red), new RectangleF(point.X - 4, point.Y - 4, 8, 8));

            #region 画弧线
            var arcR = Math.Min(Math.Abs(point.X - center.X), Math.Abs(point.Y - center.Y)) / 3;
            var angle = pLoc.Az;
            var left = center.X - arcR;
            var top = center.Y - arcR;
            Rectangle arcRect = new Rectangle((int)left, (int)top, (int)arcR * 2, ((int)arcR * 2));

            var centerAngle = Functions.DegreeToRadian(angle / 2);
            var centerPoint = new PointF((float)(arcR * Math.Sin(centerAngle) + center.X), (float)(center.Y - arcR * Math.Cos(centerAngle)) - 5);
            if (arcR > 0 && arcRect.Width > 0 && arcRect.Height > 0)
            {
                graphics.DrawArc(p, arcRect, -90, (float)angle);
                graphics.DrawString($"{angle:0.0}°", font, brush, centerPoint);
            }
            #endregion
        }

        public override void SetDisplayer(Displayer d)
        {
            base.SetDisplayer(d);
            PictureBox.MouseDown += ProcessMouseDown;
            PictureBox.MouseUp += ProcessMouseUp;
            PictureBox.MouseMove += ProcessMouseMove;
        }

        protected void ProcessMouseDown(object sender, MouseEventArgs e) => mouseDown = true;

        protected void ProcessMouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseDown)
                return;
            PPIBackground bg = displayer.Background as PPIBackground;
            var range = bg.Range;
            var r = PointFToRCoordinate(displayer.Mapper.GetCoordinateLocation(e.X, e.Y)).Polar.Dis;
            if (r > range)
                return;
            Update(e.Location, displayer.Mapper);
        }

        protected void ProcessMouseUp(object sender, MouseEventArgs e) => mouseDown = false;

        protected override void DoUpdate(PolarCoordinate data)
        {
            pLoc = data;
            rLoc = pLoc.Rectangular;
        }

        public override void Dispose()
        {
            PictureBox.MouseDown -= ProcessMouseDown;
            PictureBox.MouseUp -= ProcessMouseUp;
            PictureBox.MouseMove -= ProcessMouseMove;
        }
    }
}
