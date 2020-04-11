using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Collections.Generic;

namespace Utilities.Display
{
    public class Layer : GraphicElement
    {
        public List<GraphicElement> elements = new List<GraphicElement>();
        protected BitmapRenderTarget bitmapRt;
        public int Id { get; protected set; }
        public Layer(int id)
        {
            Id = id;
        }

        private void DrawLayerToTarget(RenderTarget rt)
        {
            rt.DrawBitmap(bitmapRt.Bitmap, 1, BitmapInterpolationMode.Linear, new RectF(0, 0, Panel.Width, Panel.Height));
        }

        private void DrawLayerOnBitmap()
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
        }

        public void DrawIfChanged(RenderTarget rt)
        {
            if (HasChanged())
                Draw(rt);
            else
                DrawLayerToTarget(rt);
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

        protected override void DrawElement(RenderTarget rt)
        {
            if (bitmapRt == null || bitmapRt.Size != rt.Size)
            {
                bitmapRt?.Dispose();
                bitmapRt = rt.CreateCompatibleRenderTarget(new CompatibleRenderTargetOptions(), rt.Size);
            }                
            bitmapRt.Transform = rt.Transform;
            DrawLayerOnBitmap();
            DrawLayerToTarget(rt);
            return;
        }
    }
}
