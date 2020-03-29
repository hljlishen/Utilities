using System.Drawing;
using System.Windows.Forms;
using Utilities.Mapper;

namespace Utilities.Display
{
    public abstract class ThreadSafeElement : SmartElement
    {
        protected readonly object Locker = new object();
        public sealed override void Draw(Graphics g, IScreenToCoordinateMapper mapper)
        {
            lock(Locker)
            {
                DoDraw(g, mapper);
            }
        }

        protected abstract void DoDraw(Graphics g, IScreenToCoordinateMapper mapper);

        public sealed override void MouseDown(object sender, MouseEventArgs e, Displayer displayer)
        {
            lock(Locker)
            {
                ProcessMouseDown(sender, e, displayer);
            }
        }

        protected virtual void ProcessMouseDown(object sender, MouseEventArgs e, Displayer displayer) { }

        public sealed override void MouseMove(object sender, MouseEventArgs e, Displayer displayer)
        {
            lock (Locker)
            {
                ProcessMouseMove(sender, e, displayer);
            }
        }

        protected virtual void ProcessMouseMove(object sender, MouseEventArgs e, Displayer displayer) { }

        public sealed override void MouseUp(object sender, MouseEventArgs e, Displayer displayer)
        {
            lock (Locker)
            {
                ProcessMouseUp(sender, e, displayer);
            }
        }

        protected virtual void ProcessMouseUp(object sender, MouseEventArgs e, Displayer displayer) { }

        //public sealed override void Update(object data)
        //{
        //    lock(Locker)
        //    {
        //        DoUpdate(data);
        //    }
        //}

        //protected abstract void DoUpdate(object data);
    }
}
