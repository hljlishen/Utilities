namespace Utilities.Mapper
{
    internal struct Area
    {
        public double Left;
        public double Right;
        public double Top;
        public double Bottom;

        public Area(double left, double right, double top, double bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }
    }
}
