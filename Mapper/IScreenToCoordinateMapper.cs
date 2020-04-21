using System;
using System.Drawing;

namespace Utilities.Mapper
{
    public interface IScreenToCoordinateMapper
    {
        event Action<IScreenToCoordinateMapper> MapperStateChanged;
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
        void SetCoordinateArea(double left, double right, double top, double bottom);
        void SetScreenArea(double left, double right, double top, double bottom);
    }
}