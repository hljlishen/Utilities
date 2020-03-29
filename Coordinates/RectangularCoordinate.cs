using System;

namespace Utilities.Coordinates
{
    public class RectangularCoordinate
    {
        public PolarCoordinate Polar => new PolarCoordinate(this);
        public RectangularCoordinate(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public RectangularCoordinate(PolarCoordinate p)
        {
            X = p.X;
            Y = p.Y;
            Z = p.Y;
        }

        public RectangularCoordinate(RectangularCoordinate r) : this(r.X, r.Y, r.Z)
        {

        }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public static double DistanceBetween(RectangularCoordinate r1, RectangularCoordinate r2)
        {
            double r = Math.Pow(r1.X - r2.X, 2) + Math.Pow(r1.Y - r2.Y, 2) + Math.Pow(r1.Z - r2.Z, 2);
            return Math.Sqrt(r);
        }

        public double DistanceTo(RectangularCoordinate r) => DistanceBetween(this, r);
    }
}
