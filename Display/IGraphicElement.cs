using System.Drawing;
using System.Windows.Forms;
using Utilities.Mapper;

namespace Utilities.Display
{
    public interface IGraphicElement
    {
        void Draw(Graphics g, IScreenToCoordinateMapper mapper);
        void Update(object data);
        void MouseDown(object sender, MouseEventArgs e, Displayer displayer);
        void MouseUp(object sender, MouseEventArgs e, Displayer displayer);
        void MouseMove(object sender, MouseEventArgs e, Displayer displayer);
    }
}
