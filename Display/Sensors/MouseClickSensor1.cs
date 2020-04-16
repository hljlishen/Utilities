using System.Windows.Forms;

namespace Utilities.Display
{
    public class MouseClickSensor1 : Sensor
    {
        public override void SetDisplayer(Displayer d)
        {
            base.SetDisplayer(d);
            Panel.MouseClick += Panel_MouseClick;
        }

        private void Panel_MouseClick(object sender, MouseEventArgs e)
        {
            bool stateChanged = false;
            lock (locker)
            {
                if (objects == null || objects.Count == 0)
                    return;

                foreach (var o in objects)
                {
                    if (o.IsPointNear(e.Location))
                    {
                        o.MouseLocation = e.Location;
                        o.Selected = !o.Selected;
                        stateChanged = true;
                    }
                }
            }
            if (stateChanged)
                InvokeObjectStateChanged();
        }

        public override void Dispose()
        {
            base.Dispose();
            Panel.MouseClick -= Panel_MouseClick;
        }
    }
}
