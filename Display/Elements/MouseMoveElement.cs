namespace Utilities.Display
{
    public abstract class MouseMoveElement<ObjectType, UpdateType> : MouseSensitiveElement<ObjectType, UpdateType> where ObjectType : MouseSensitiveObject
    {
        public override void SetDisplayer(Displayer d)
        {
            base.SetDisplayer(d);
            Panel.MouseMove += Panel_MouseMove;
        }

        private void Panel_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e) => ProcessMouseEvent(e);
    }
}
