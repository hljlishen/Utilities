using System.Windows.Forms;

namespace Utilities.Display
{
    public abstract class MouseClickToCancelSelectionElement<T, UpdateType> : MouseSensitiveElement<T, UpdateType> where T : LiveObject
    {
        public override void SetDisplayer(Displayer d)
        {
            base.SetDisplayer(d);
            Panel.MouseClick += Panel_MouseClick;
        }

        private void Panel_MouseClick(object sender, MouseEventArgs e)
        {
            ProcessMouseEvent(e);
        }

        protected override void ProcessMouseEvent(MouseEventArgs e)
        {
            lock (Locker)
            {
                foreach (var o in objects)
                {
                    if (o.IsPointNear(e.Location))
                    {
                        o.MouseLocation = e.Location;
                        o.Selected = !o.Selected;
                        MouseClickLiveObjectHandler(e, o);
                        UpdateGraphic();
                    }
                }
            }
        }

        protected abstract void MouseClickLiveObjectHandler(MouseEventArgs e, T t);
    }
}
