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
        void SetCoordinateXRange(double xLeft, double xRight);
        void SetCoordinateYRange(double yTop, double yBottom);
        void SetCoordinateArea(double left, double right, double top, double bottom);
        void SetScreenArea(double left, double right, double top, double bottom);
    }

    public abstract class MapperBase : IScreenToCoordinateMapper
    {
        public abstract double ScreenLeft { get; }
        public abstract double ScreenRight { get; }
        public abstract double ScreenTop { get; }
        public abstract double ScreenBottom { get; }
        public abstract PointF ScreenCenter { get; }
        public abstract double ScreenWidth { get; }
        public abstract double ScreenHeight { get; }
        public abstract double CoordinateLeft { get; }
        public abstract double CoordinateRight { get; }
        public abstract double CoordinateTop { get; }
        public abstract double CoordinateBottom { get; }

        public event Action<IScreenToCoordinateMapper> MapperStateChanged;
        protected void InvokeStateChange() => MapperStateChanged?.Invoke(this);

        public abstract PointF GetCoordinateLocation(double screenX, double screenY);
        public abstract double GetCoordinateX(double screenX);
        public abstract double GetCoordinateY(double screenY);
        public abstract PointF GetScreenLocation(double coordinateX, double coordinateY);
        public abstract double GetScreenX(double coordinateX);
        public abstract double GetScreenY(double coordinateY);
        public virtual void SetCoordinateArea(double left, double right, double top, double bottom)
        {
            InvokeStateChange();
        }
        public virtual void SetCoordinateXRange(double xLeft, double xRight)
        {
            InvokeStateChange();
        }
        public virtual void SetCoordinateYRange(double yTop, double yBottom)
        {
            InvokeStateChange();
        }
        public virtual void SetScreenArea(double left, double right, double top, double bottom)
        {
            InvokeStateChange();
        }
    }
}