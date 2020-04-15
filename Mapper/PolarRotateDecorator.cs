using System;
using System.Drawing;
using Utilities.Tools;
using Utilities.Coordinates;

namespace Utilities.Mapper
{
    public class PolarRotateDecorator : MapperDecorator
    {
        private double rotateAngle = 0;

        private static PolarRotateDecorator Instance = null;

        public static PolarRotateDecorator GetInstance(IScreenToCoordinateMapper mapper)
        {
            if (Instance == null)
                Instance = new PolarRotateDecorator(mapper);
            return Instance;
        }

        public double RotateAngle
        {
            get => rotateAngle;
            set
            {
                rotateAngle = value;
                InvokeStateChanged();
            }
        }
        public PointF Center { get; set; }
        public PolarRotateDecorator(IScreenToCoordinateMapper mapper) : base(mapper)
        {
            mapper.MapperStateChanged += Mapper_MapperStateChanged;
            Center = mapper.GetScreenLocation(0, 0);
        }

        private void Mapper_MapperStateChanged(IScreenToCoordinateMapper obj)
        {
            Center = Mapper.GetScreenLocation(0, 0);
            InvokeStateChanged();
        }

        public override double GetScreenX(double coordinateX)
        {
            throw new Exception("PolarRotateDecorator不实现该函数");
        }

        public override double GetScreenY(double coordinateY)
        {
            throw new Exception("PolarRotateDecorator不实现该函数");
        }

        public override PointF GetScreenLocation(double coordinateX, double coordinateY)
        {
            var p = base.GetScreenLocation(coordinateX, coordinateY);
            //var radian = Functions.DegreeToRadian(RotateAngle);
            //var x0 = (p.X - Center.X) * Math.Cos(radian) - (p.Y - Center.Y) * Math.Sin(radian) + Center.X;
            //var y0 = (p.X - Center.X) * Math.Sin(radian) - (p.Y - Center.Y) * Math.Cos(radian) + Center.Y;
            //return new PointF((float)x0, (float)y0);
            PolarCoordinate p1 = new RectangularCoordinate(p.X - Center.X, Center.Y - p.Y, 0).Polar;
            p1.Az -= RotateAngle;
            p1.Az = Functions.StandardAngle(p1.Az);
            var r = p1.Rectangular;
            return new PointF((float)r.X + Center.X, Center.Y + (float)r.Y);
        }
    }
}
