using System.Drawing;
using System.Windows.Forms;
using Utilities.Mapper;

namespace Utilities.Display
{
    public class SmartElement : IGraphicElement
    {
        protected bool Changed = true;

        public virtual bool HasChanged()
        {
            return Changed;
        }

        public virtual void Draw(Graphics g, IScreenToCoordinateMapper mapper)
        {
            Changed = false;
        }

        public virtual void MouseDown(object sender, MouseEventArgs e, Displayer displayer)
        {
        }

        public virtual void MouseMove(object sender, MouseEventArgs e, Displayer displayer)
        {
        }

        public virtual void MouseUp(object sender, MouseEventArgs e, Displayer displayer)
        {
        }

        public virtual void Update(object data)
        {
            Changed = true;
        }
    }
}
