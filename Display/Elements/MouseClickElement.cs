namespace Utilities.Display
{
    public abstract class MouseClickElement<ObjectType, UpdateType> : MouseSensitiveElement<ObjectType, UpdateType> where ObjectType : MouseSensitiveObject
    {
        public override void SetDisplayer(Displayer d)
        {
            base.SetDisplayer(d);
            Panel.MouseClick += Panel_MouseClick;
        }

        private void Panel_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e) => ProcessMouseEvent(e);
    }
}
