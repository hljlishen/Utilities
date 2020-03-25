namespace Utilities.Mapper
{
    public class MapperFactory
    {
        public static IScreenToCoordinateMapper GetMapper() => new ScreenToCoordinateMapper();

        public static IScreenToCoordinateMapper GetMapper(double screenLeft, double screenRight, double coordinateXLeft, double coordinateXRight, double screenTop, double screenBottom, double coordinateYTop, double coordinateYBottom) => new ScreenToCoordinateMapper(screenLeft, screenRight, coordinateXLeft, coordinateXRight, screenTop, screenBottom, coordinateYTop, coordinateYBottom);
    }
}
