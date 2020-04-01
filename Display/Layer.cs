using System.Collections.Generic;
using System.Drawing;
using Utilities.Mapper;

namespace Utilities.Display
{
    public class Layer : GraphicElement
    {
        public List<GraphicElement> elements = new List<GraphicElement>();
        protected Bitmap bmp;
        public int Id { get; protected set; }
        public Layer(int id)
        {
            Id = id;
        }

        public override void Draw(Graphics g)
        {
            DrawElements(Mapper);
            DrawBitmap(g);
            base.Draw(g);
            return;
        }

        private void DrawBitmap(Graphics g) => g.DrawImage(bmp, new Point(0, 0));

        private void DrawElements(IScreenToCoordinateMapper mapper)
        {
            bmp?.Dispose();
            bmp = new Bitmap((int)mapper.ScreenWidth, (int)mapper.ScreenHeight);
            bmp.MakeTransparent();
            using(var graphics = Graphics.FromImage(bmp))
            {
                lock (Locker)
                {
                    foreach (var e in elements)
                    {
                        e.Draw(graphics);
                    }
                }
            }
            base.Draw(null);
        }

        public void DrawIfChanged(Graphics g)
        {
            if (HasChanged())
                Draw(g);
            else
                DrawBitmap(g);
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
        }
    }
}
