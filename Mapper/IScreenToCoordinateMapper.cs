using System.Drawing;

namespace Utilities.Mapper
{
    public interface IScreenToCoordinateMapper
    {
        //ValueMapper XAxisMapper { get; }
        //ValueMapper YAxisMapper { get; }
        double ScreenLeft { get; }
        double ScreenRight { get; }
        double ScreenTop { get; }
        double ScreenBottom { get; }
        PointF ScreenCenter { get; }
        double ScreenWidth { get; }
        double ScreenHeight { get; }
        double CoordinateLeft { get; }
        double CoordinateRight { get; }
        double CoordinateTop { get; }
        double CoordinateBottom { get; }
        PointF GetCoordinateLocation(double screenX, double screenY);
        double GetCoordinateX(double screenX);
        double GetCoordinateY(double screenY);
        PointF GetScreenLocation(double coordinateX, double coordinateY);
        double GetScreenX(double coordinateX);
        double GetScreenY(double coordinateY);
        void SetCoordinateXRange(double xLeft, double xRight);
        void SetCoordinateYRange(double yTop, double yBottom);
        void SetScreenArea(double left, double right, double top, double bottom);
    }
}