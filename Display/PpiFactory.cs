using System;
using Utilities.Mapper;

namespace Utilities.Display
{
    public class PpiFactory : AbstractDisplayerFactory
    {
        public PpiFactory(BackgroundModel initialBackgroundModel) : base(initialBackgroundModel)
        {
            background = new PPIBackground();
            background.Update(initialBackgroundModel);
            mapper = new SquaredScreenRectDecorator(new ScreenToCoordinateMapper());
            zoomCtrl = new ZoomController(RectSelectType.Square);
        }
    }
}
