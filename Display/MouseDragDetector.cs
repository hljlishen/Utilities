using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utilities.Display
{
    public class MouseDragDetector : IDisposable
    {
        private bool mouseDown = false;
        private Point mouseDownPos;
        private Point mouseCurrentPos;
        public MouseDragDetector(Panel panel)
        {
            Panel = panel;
            Panel.MouseDown += Panel_MouseDown;
            //Panel.MouseMove += Panel_MouseMove;
            Panel.MouseUp += Panel_MouseUp;
            Panel.MouseDoubleClick += Panel_MouseDoubleClick;
        }

        private void Panel_MouseDoubleClick(object sender, MouseEventArgs e) => mouseDown = false;

        private void Panel_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            Panel.MouseMove -= Panel_MouseMove;
        }

        private void Panel_MouseMove(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Panel_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            mouseDownPos = e.Location;
            mouseCurrentPos = e.Location;
            Panel.MouseMove += Panel_MouseMove;
        }

        public void Dispose()
        {
            Panel.MouseDown -= Panel_MouseDown;
            Panel.MouseUp -= Panel_MouseUp;
            Panel.MouseDoubleClick -= Panel_MouseDoubleClick;
        }

        public Panel Panel { get; private set; }
    }
}
