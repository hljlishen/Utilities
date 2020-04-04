using System.Drawing;
using Utilities.Coordinates;

namespace Utilities.Display
{
    public class PpiAntenna : DynamicElement<double>
    {
        private double degree = 0;
        public Pen AntennaPen = new Pen(Color.White, 2);
        protected override void DoDraw(Graphics g)
        {
            RectangularCoordinate p = new PolarCoordinate(degree, 0, (Background as PPIBackground).Range).Rectangular;
            PointF endPoint = Mapper.GetScreenLocation(p.X, p.Y);
            PointF beginPoint = Mapper.GetScreenLocation(0, 0);
            g.DrawLine(AntennaPen, beginPoint, endPoint);
        }

        protected override void DoUpdate(double t)
        {
            degree = t;
        }
    }
}
