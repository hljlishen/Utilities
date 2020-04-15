using Utilities.Mapper;

namespace Utilities.Display
{
    public abstract class RotatableElement<T> : DynamicElement<T>
    {
        protected static PolarRotateDecorator rotateDecorator = null;

        public override void SetDisplayer(Displayer d)
        {
            base.SetDisplayer(d);

            if(rotateDecorator == null)
            {
                rotateDecorator = PolarRotateDecorator.GetInstance(d.Mapper);
            }

            Mapper.MapperStateChanged += Mapper_MapperStateChanged;
        }

        private void Mapper_MapperStateChanged(IScreenToCoordinateMapper obj)
        {
            UpdateGraphic();
        }

        public static void SetRotateAngle(double angle) => rotateDecorator.RotateAngle = angle;

        public override IScreenToCoordinateMapper Mapper => rotateDecorator;
    }
}
