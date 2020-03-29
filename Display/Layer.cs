using System.Collections.Generic;
using System.Drawing;
using Utilities.Mapper;

namespace Utilities.Display
{
    public class Layer : ThreadSafeElement
    {
        public List<SmartElement> elements = new List<SmartElement>();
        protected Bitmap bmp;
        public int Id { get; protected set; }
        public Layer(int id)
        {
            Id = id;
        }

        protected override void DoDraw(Graphics g, IScreenToCoordinateMapper mapper)
        {
            base.Draw(g, mapper);

            if(HasChanged())
            {
                bmp?.Dispose();
                bmp = new Bitmap((int)mapper.ScreenWidth, (int)mapper.ScreenHeight);
                var graphics = Graphics.FromImage(bmp);
                DrawElements(graphics, mapper);
            }                
            g.DrawImage(bmp, new Point(0, 0));
            return;
        }

        protected virtual void DrawElements(Graphics g, IScreenToCoordinateMapper mapper)
        {
            foreach (var e in elements)
            {
                e.Draw(g, mapper);
            }
        }

        public override bool HasChanged()
        {
            if (base.HasChanged())
                return true;
            return ElementsChanged();
        }

        private bool ElementsChanged()
        {
            if (elements.Count == 0)
                return false;
            foreach (var e in elements)
            {
                if (e.HasChanged())
                    return true;
            }
            return false;
        }

        public void AddElement(SmartElement e)
        {
            elements.Add(e);
            Changed = true;
        }

        public void RemoveElement(SmartElement e)
        {
            if(elements.Contains(e))
            {
                elements.Remove(e);
                Changed = true;
            }
        }
    }
}
