﻿using System.Windows.Forms;

namespace Utilities.Display
{
    public class MouseClickSensor1 : Sensor
    {
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

        protected override void BindEvents(Panel panel) => panel.MouseClick += Panel_MouseClick;

        protected override void UnbindEvents(Panel panel) => panel.MouseClick -= Panel_MouseClick;
    }
}
