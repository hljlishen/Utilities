using System;
using Utilities.Mapper;

namespace Utilities.Display
{
    public class PpiFactory : AbstractDisplayerFactory
    {
        public PpiFactory(BackgroundModel initialBackgroundModel) : base(initialBackgroundModel)
        {
            background = new PpiBackground();
            background.Update(initialBackgroundModel);
            mapper = new SquaredScreenRectDecorator(new ScreenToCoordinateMapper());
            zoomCtrl = new ZoomController(RectSelectType.Square);
        }
    }
}
