using Utilities.Mapper;

namespace Utilities.Display
{
    public abstract class AbstractDisplayerFactory
    {
        public BackgroundModel InitialBackgroundModel { get; set; }
        public abstract Background GetBackground();

        public abstract IScreenToCoordinateMapper GetMapper();

        public abstract ZoomController GetZoomController();
    }
}
