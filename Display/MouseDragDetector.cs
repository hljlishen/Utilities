using System;
using System.Drawing;
using System.Windows.Forms;

namespace Utilities.Display
{
    public class MouseDragDetector : IDisposable
    {
        private bool mouseDown = false;
        private Point mouseDownPos;
        private Point mouseCurrentPos;

        public event Action<Point, Point> MouseDrag;
        public event Action<Point> MouseUp;
        public MouseDragDetector(Panel panel)
        {
            Panel = panel;
            Panel.MouseDown += Panel_MouseDown;
            Panel.MouseUp += Panel_MouseUp;
            Panel.MouseDoubleClick += Panel_MouseDoubleClick;
        }

        private void Panel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            try
            {
                Panel.MouseMove -= Panel_MouseMove;
            }
            catch
            {

            }
        }

        private void Panel_MouseUp(object sender, MouseEventArgs e)
        {
            if (!mouseDown)
                return;
            mouseDown = false;
            Panel.MouseMove -= Panel_MouseMove;
            MouseUp?.Invoke(e.Location);
        }

        private void Panel_MouseMove(object sender, MouseEventArgs e)
        {
            mouseCurrentPos = e.Location;
            MouseDrag?.Invoke(mouseDownPos, mouseCurrentPos);
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
