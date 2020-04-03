using Utilities.Mapper;

namespace Utilities.Display
{
    public class PpiFactory : AbstractDisplayerFactory
    {
        public override Background GetBackground() => new PPIBackground(InitialBackgroundModel);

        public override IScreenToCoordinateMapper GetMapper() => new SquaredScreenRectDecorator(new ScreenToCoordinateMapper());

        public override ZoomController GetZoomController() => new ZoomController(RectSelectType.Square);
    }
}
