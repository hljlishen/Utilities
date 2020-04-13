using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.Display
{
    public struct ButtenProperties
    {
        public Point Location;
        public Size Size;
        public Color ForeColor;
        public Color SelectedColor;
        public Color FrameColor;
        public int FrameWidth;
        public string Text;

        public ButtenProperties(Point location, Size size, string text) : this(location, size, Color.Gray, Color.Yellow, Color.White, 2, text)
        {
        }

        public ButtenProperties(Point location, Size size, Color foreColor, Color selectedColor, Color frameColor, int frameWidth, string text)
        {
            Location = location;
            Size = size;
            ForeColor = foreColor;
            SelectedColor = selectedColor;
            FrameColor = frameColor;
            FrameWidth = frameWidth;
            Text = text;
        }
    }
    public class ButtonElement : MouseClickToCancelSelectionElement<LiveRect, ButtenProperties>
    {
        protected Brush ForeBrush;
        protected Brush SelectedBrush;
        protected Brush FrameBrush;
        private bool firstDraw = true;
        private bool IsSelected;

        public bool Selected
        {
            get => objects[0].Selected;
            set => objects[0].Selected = value;
        }


        public ButtonElement(ButtenProperties buttenProperties)
        {
            Model = buttenProperties;
        }

        public event Action<ButtonElement> Clicked;

        public override void Dispose()
        {
            base.Dispose();
            ForeBrush.Dispose();
            SelectedBrush.Dispose();
            FrameBrush.Dispose();
        }

        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            ForeBrush = Model.ForeColor.SolidBrush(rt);
            SelectedBrush = Model.SelectedColor.SolidBrush(rt);
            FrameBrush = Model.FrameColor.SolidBrush(rt);
        }

        protected override void DrawObjectSelected(RenderTarget rt, LiveRect o)
        {
            RoundedRect rect = new RoundedRect(o.Rectangle.ToRectF(), o.Rectangle.Width / 10.0f, o.Rectangle.Height / 10.0f);
            rt.DrawRoundedRectangle(rect, FrameBrush, 2.5f);
            rt.FillRoundedRectangle(rect, SelectedBrush);
            IsSelected = true;
        }

        protected override void DrawObjectUnselected(RenderTarget rt, LiveRect o)
        {
            RoundedRect rect = new RoundedRect(o.Rectangle.ToRectF(), o.Rectangle.Width / 10.0f, o.Rectangle.Height / 10.0f);
            rt.DrawRoundedRectangle(rect, FrameBrush, 2.5f);
            rt.FillRoundedRectangle(rect, ForeBrush);
            IsSelected = false;
            if(firstDraw)
            {
                firstDraw = false;
                return;
            }
        }

        protected override IEnumerable<LiveRect> GetObjects()
        {
            LiveRect r = new LiveRect(new Rectangle(Model.Location, Model.Size));
            r.Selected = IsSelected;
            yield return r;
        }

        protected override void MouseClickLiveObjectHandler(MouseEventArgs e, LiveRect t)
        {
            Clicked?.Invoke(this);
        }
    }
}
