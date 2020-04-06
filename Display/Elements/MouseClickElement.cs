namespace Utilities.Display
{
    public abstract class MouseClickElement<T> : MouseSensitiveElement<T> where T : MouseSensitiveObject
    {
        protected MouseClickElement(MarkerModel model) : base(model)
        {
        }

        public override void SetDisplayer(Displayer d)
        {
            base.SetDisplayer(d);
            Panel.MouseClick += Panel_MouseClick;
        }

        private void Panel_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e) => ProcessMouseEvent(e);
    }
}
