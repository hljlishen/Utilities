using System.Drawing;
using Utilities.Mapper;

namespace Utilities.Display
{
    public abstract class DynamicElement<T> : GraphicElement
    {
        public override void Draw(Graphics g)
        {
            lock(Locker)
            {
                DoDraw(g);
                base.Draw(g);
            }
        }

        protected abstract void DoDraw(Graphics g);

        public virtual void Update(T t)
        {
            lock(Locker)
            {
                DoUpdate(t);
                Changed = true;
            }
        }

        protected abstract void DoUpdate(T t);
    }
}
