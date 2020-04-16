using Utilities.Mapper;

namespace Utilities.Display
{
    public abstract class RotatableElement<T> : DynamicElement<T>
    {
        protected static PolarRotateDecorator rotateDecorator = null;
        protected string RotateDecoratorName;

        protected RotatableElement(string rotateDecoratorName = "default")
        {
            RotateDecoratorName = rotateDecoratorName;
        }

        public double RotateAngle => rotateDecorator.RotateAngle;

        public override void SetDisplayer(Displayer d)
        {
            if(rotateDecorator == null)
            {
                rotateDecorator = PolarRotateDecorator.GetInstance(RotateDecoratorName, d.Mapper);
            }

            Mapper.MapperStateChanged += Mapper_MapperStateChanged;
            base.SetDisplayer(d);
        }

        private void Mapper_MapperStateChanged(IScreenToCoordinateMapper obj) => UpdateGraphic();

        public override IScreenToCoordinateMapper Mapper => rotateDecorator;

        public IScreenToCoordinateMapper InnerMapper => rotateDecorator.Mapper;
    }
}
