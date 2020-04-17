using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Drawing;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;
//using Font = System.Drawing.Font;

namespace Utilities.Display
{
    public class ButtonElement : DynamicElement<ButtenProperties>
    {
        protected Brush ForeFillBrush;
        protected Brush SelectedFillBrush;
        protected Brush FrameBrush;
        protected TextFormat ForeTextFormat;
        protected Brush ForeTextBrush;
        protected TextFormat SelectedTextFormat;
        protected Brush SelectedTextBrush;
        private bool IsSelected;
        private PointF center;
        private TextLayout layout;
        private Point2F textLocation;

        public bool Selected
        {
            get => Objects[0].Selected;
            set => Objects[0].Selected = value;
        }

        public ButtonElement(ButtenProperties buttenProperties)
        {
            Model = buttenProperties;
            center.X = Model.Location.X + Model.ForeSize.Width / 2;
            center.Y = Model.Location.Y + Model.ForeSize.Height / 2;
            Sensor = new MouseClickSensor1();
        }

        public event Action<ButtonElement> Clicked;

        public override void Dispose()
        {
            base.Dispose();
            ForeFillBrush.Dispose();
            SelectedFillBrush.Dispose();
            FrameBrush.Dispose();
            ForeTextBrush.Dispose();
            SelectedTextFormat.Dispose();
            SelectedTextBrush.Dispose();
            ForeTextFormat.Dispose();
        }

        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            ForeFillBrush = Model.ForeColor.SolidBrush(rt);
            SelectedFillBrush = Model.SelectedColor.SolidBrush(rt);
            FrameBrush = Model.FrameColor.SolidBrush(rt);
            ForeTextBrush = Model.ForeFontColor.SolidBrush(rt);
            SelectedTextBrush = Model.SelectedFontColor.SolidBrush(rt);
            ForeTextFormat = Model.ForeFontName.MakeFormat(Model.ForeFontSize);
            SelectedTextFormat = Model.SelectedFontName.MakeFormat(Model.SelectedFontSize);

            var dw = DWriteFactory.CreateFactory();
            layout = dw.CreateTextLayout(Model.Text, ForeTextFormat, Model.ForeSize.Width, Model.ForeSize.Height);
            layout.TextAlignment = TextAlignment.Center;
            var height = layout.Metrics.Height;
            var offset = (layout.Metrics.LayoutHeight - layout.Metrics.Height) / 2;
            textLocation = new Point2F(Model.Location.X, Model.Location.Y + offset);
            dw.Dispose();
        }

        protected void DrawObjectSelected(RenderTarget rt, LiveRect o)
        {
            rt.DrawRectangle(o.Rectangle.ToRectF(), FrameBrush, 4f);
            rt.FillRectangle(o.Rectangle.ToRectF(), SelectedFillBrush);
            rt.DrawTextLayout(textLocation, layout, SelectedTextBrush);
            IsSelected = true;
        }

        protected void DrawObjectUnselected(RenderTarget rt, LiveRect o)
        {
            rt.DrawRectangle(o.Rectangle.ToRectF(), FrameBrush, 2.5f);
            rt.FillRectangle(o.Rectangle.ToRectF(), ForeFillBrush);
            rt.DrawTextLayout(textLocation, layout, ForeTextBrush);
            IsSelected = false;
        }

        protected override IEnumerable<LiveObject> GetObjects()
        {
            LiveRect r = new LiveRect(new Rectangle(Model.Location, Model.ForeSize));

            r.Selected = IsSelected;
            yield return r;
        }
        protected override void Sensor_ObjectStateChanged(Sensor obj)
        {
            Clicked?.Invoke(this);
            base.Sensor_ObjectStateChanged(obj);
        }

        protected override void DrawDynamicElement(RenderTarget rt)
        {
            foreach (var o in Objects)
            {
                var l = o as LiveRect;
                if (l.Selected)
                    DrawObjectSelected(rt, l);
                else
                    DrawObjectUnselected(rt, l);
            }
        }
    }
}
