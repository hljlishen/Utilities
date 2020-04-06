namespace Utilities.Display
{
    public abstract class MouseMoveElement<T> : MouseSensitiveElement<T> where T : MouseSensitiveObject
    {
        protected MouseMoveElement(MarkerModel model) : base(model)
        {
        }

        public override void SetDisplayer(Displayer d)
        {
            base.SetDisplayer(d);
            Panel.MouseMove += Panel_MouseMove;
        }

        private void Panel_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e) => ProcessMouseEvent(e);
    }
}
