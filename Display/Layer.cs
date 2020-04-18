using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Collections.Generic;

namespace Utilities.Display
{
    public class Layer : GraphicElement
    {
        public List<GraphicElement> elements = new List<GraphicElement>();
        protected BitmapRenderTarget bitmapRt;

        public Layer(int id)
        {
            LayerId = id;
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

        public void Add(GraphicElement e)
        {
            lock (Locker)
                elements.Add(e);
            e.LayerId = LayerId;
            e.SetDisplayer(displayer);
            UpdateView();
        }

        public void AddRange(IEnumerable<GraphicElement> es)
        {
            lock(Locker)
            {
                foreach(var e in es)
                {
                    elements.Add(e);
                    e.LayerId = LayerId;
                    e.SetDisplayer(displayer);
                }
                UpdateView();
            }
        }

        /// <summary>
        /// 将图层中的所有元素替换为参数中的新元素
        /// </summary>
        /// <param name="es">要显示在图层中的元素</param>
        public void RefreshLayerElements(IEnumerable<GraphicElement> es)
        {
            lock (Locker)
            {
                //清空当前图层的所有元素
                foreach(var e in elements)
                {
                    e.Dispose();
                }
                elements?.Clear();

                //给图层添加新元素
                foreach (var e in es)
                {
                    elements.Add(e);
                    e.LayerId = LayerId;
                    e.SetDisplayer(displayer);
                }
                UpdateView();
            }
        }

        public void RemoveElement(GraphicElement e)
        {
            lock (Locker)
            {
                if (elements.Contains(e))
                {
                    elements.Remove(e);
                    UpdateView();
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
        protected override IEnumerable<LiveObject> GetObjects() => null;
    }
}
