using System.Windows.Forms;

namespace Utilities.Display
{
    public class MouseMoveSensor : Sensor
    {
        public override void SetDisplayer(Displayer d)
        {
            base.SetDisplayer(d);
            Panel.MouseMove += Panel_MouseMove;
        }

        private void Panel_MouseMove(object sender, MouseEventArgs e)
        {
            lock(locker)
            {
                foreach (var o in objects)
                {
                    if (o.IsPointNear(e.Location))
                    {
                        o.MouseLocation = e.Location;
                        if (!o.Selected)
                        {
                            o.Selected = true;
                            InvokeObjectStateChanged();
                        }
                    }
                    else
                    {
                        if (o.Selected)
                        {
                            o.Selected = false;
                            InvokeObjectStateChanged();
                        }
                    }
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            Panel.MouseMove -= Panel_MouseMove;
        }
    }
}
