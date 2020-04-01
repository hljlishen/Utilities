using System;
using System.Drawing;
using System.Windows.Forms;
using Utilities.Mapper;

namespace Utilities.Display
{
    public abstract class GraphicElement : IDisposable
    {
        protected bool Changed = true;
        protected Displayer displayer;
        protected readonly object Locker = new object();
        public virtual void Draw(Graphics g/*, IScreenToCoordinateMapper mapper*/)
        {
            Changed = false;
        }
        public virtual bool HasChanged()=> Changed;
        public virtual void SetDisplayer(Displayer d) => displayer = d;
        protected PictureBox PictureBox => displayer.PictureBox;
        protected IScreenToCoordinateMapper Mapper => displayer.Mapper;
        public virtual void Dispose() { }
    }
}
