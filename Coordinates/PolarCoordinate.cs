using System;
using Utilities.Tools;

namespace Utilities.Coordinates
{
    public class PolarCoordinate
    {
        private double az;   //方位
        public double Az
        {
            get => az;

            set => az = Functions.StandardAngle(value);
        }

        public double El { get; set; }

        public double Dis { get; set; }

        public double ProjectedDis => Dis * Math.Cos(Functions.DegreeToRadian(El));

        public PolarCoordinate()
        {
            Az = -1;
            El = -1;
            Dis = -1;
        }

        public PolarCoordinate(double az, double el, double dis)
        {
            Az = az;
            El = el;
            Dis = dis;
        }

        public PolarCoordinate(RectangularCoordinate r)
        {
            Dis = Math.Sqrt(r.X * r.X + r.Y * r.Y + r.Z * r.Z);
            El = 90 - Functions.RadianToDegree(Math.Acos(r.Z / Dis));
            Az = 90 - Functions.RadianToDegree(Math.Atan2(r.Y, r.X));
        }

        public PolarCoordinate(PolarCoordinate c)
        {
            Az = c.Az;
            El = c.El;
            Dis = c.Dis;
        }

        public PolarCoordinate Copy() => new PolarCoordinate(this);

        public double X => (Dis * Math.Cos(Functions.DegreeToRadian(El)) * Math.Sin(Functions.DegreeToRadian(Az)));

        public double Y => (Dis * Math.Cos(Functions.DegreeToRadian(El)) * Math.Cos(Functions.DegreeToRadian(Az)));

        public double Z => (Dis * Math.Sin(Functions.DegreeToRadian(El)));

        public RectangularCoordinate Rectangular => new RectangularCoordinate(X, Y, Z);

        public static double DistanceBetween(PolarCoordinate c1, PolarCoordinate c2) => c1.Rectangular.DistanceTo(c2.Rectangular);

        public double DistanceTo(PolarCoordinate c) => DistanceBetween(this, c);

        public static PolarCoordinate RetangularToPolar(double x, double y, double z)
        {
            var ret = new PolarCoordinate
            {
                Dis = Math.Sqrt(x * x + y * y + z * z)
            };
            ret.El = 90 - Functions.RadianToDegree(Math.Acos(z / ret.Dis));
            ret.Az = 90 - Functions.RadianToDegree(Math.Atan(y / x));
            return ret;
        }
    }
}
