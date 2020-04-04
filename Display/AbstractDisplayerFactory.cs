using Utilities.Mapper;

namespace Utilities.Display
{
    public class AbstractDisplayerFactory
    {
        protected Background background;
        protected bool firstTimeGetBackground = true;
        protected IScreenToCoordinateMapper mapper;
        protected ZoomController zoomCtrl;

        public AbstractDisplayerFactory(BackgroundModel initialBackgroundModel)
        {
            InitialBackgroundModel = initialBackgroundModel;
        }

        public BackgroundModel InitialBackgroundModel { get; set; }
        public virtual Background GetBackground() => background;
        public virtual IScreenToCoordinateMapper GetMapper() => mapper;
        public virtual ZoomController GetZoomController() => zoomCtrl;
    }
}
