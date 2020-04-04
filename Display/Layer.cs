using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Collections.Generic;
using System.Drawing;
using Utilities.Mapper;

namespace Utilities.Display
{
    public class Layer : GraphicElement
    {
        public List<GraphicElement> elements = new List<GraphicElement>();
        protected Bitmap bmp;
        protected BitmapRenderTarget bitmapRt;
        public int Id { get; protected set; }
        public Layer(int id)
        {
            Id = id;
        }

        public override void Draw(RenderTarget rt)
        {
            if (bitmapRt == null)
                bitmapRt = rt.CreateCompatibleRenderTarget(new CompatibleRenderTargetOptions(), new Microsoft.WindowsAPICodePack.DirectX.Direct2D1.SizeF(PictureBox.Width, PictureBox.Height));
            DrawElements(rt, Mapper);
            DrawBitmap(rt);
            base.Draw(rt);
            return;
        }

        private void DrawBitmap(RenderTarget rt)
        {
            rt.DrawBitmap(bitmapRt.Bitmap);
        }

        private void DrawElements(RenderTarget rt, IScreenToCoordinateMapper mapper)
        {
            bitmapRt.BeginDraw();
            bitmapRt.Clear();

            lock (Locker)
            {
                foreach (var e in elements)
                {
                    e.Draw(bitmapRt);
                }
            }
            bitmapRt.EndDraw();
            base.Draw(null);
        }

        public void DrawIfChanged(RenderTarget rt)
        {
            if (HasChanged())
                Draw(rt);
            else
                DrawBitmap(rt);
        }

        public override bool HasChanged() => base.HasChanged() ? true : ElementsChanged();

        private bool ElementsChanged()
        {
            if (elements.Count == 0)
                return false;
            lock (Locker)
            {
                foreach (var e in elements)
                {
                    if (e.HasChanged())
                        return true;
                }
            }

            return false;
        }

        public void AddElement(GraphicElement e)
        {
            lock (Locker)
                elements.Add(e);
            e.SetDisplayer(displayer);
            Changed = true;
        }

        public void RemoveElement(GraphicElement e)
        {
            lock (Locker)
            {
                if (elements.Contains(e))
                {
                    elements.Remove(e);
                    Changed = true;
                }
            }
        }

        public void Clear()
        {
            lock (Locker)
            {
                elements.Clear();
            }
        }

        public override void Dispose()
        {
            foreach (var e in elements)
            {
                e.Dispose();
            }
            elements.Clear();
            bitmapRt?.Dispose();
        }
    }
}
