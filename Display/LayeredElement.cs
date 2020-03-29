using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Utilities.Mapper;

namespace Utilities.Display
{
    public class LayeredElement : ThreadSafeElement
    {
        protected Dictionary<uint, List<IGraphicElement>> layers = new Dictionary<uint, List<IGraphicElement>>();

        protected virtual void DrawLayer(uint key, Graphics g, IScreenToCoordinateMapper mapper)
        {
            foreach (var e in layers[key])
            {
                e?.Draw(g, mapper);
            }
        }

        public void AddElement(uint layer, IGraphicElement element)
        {
            if (layers.ContainsKey(layer))
                layers[layer].Add(element);
            else
                layers.Add(layer, new List<IGraphicElement>() { element });
        }

        protected override void ProcessMouseDown(object sender, MouseEventArgs e, Displayer displayer)
        {
            foreach(var k in layers.Keys)
            {
                foreach (var el in layers[k])
                {
                    el.MouseDown(sender, e, displayer);
                }
            }
        }

        protected override void ProcessMouseUp(object sender, MouseEventArgs e, Displayer displayer)
        {
            foreach (var k in layers.Keys)
            {
                foreach (var el in layers[k])
                {
                    el.MouseUp(sender, e, displayer);
                }
            }
        }

        protected override void ProcessMouseMove(object sender, MouseEventArgs e, Displayer displayer)
        {
            foreach (var k in layers.Keys)
            {
                foreach (var el in layers[k])
                {
                    el.MouseMove(sender, e, displayer);
                }
            }
        }

        protected override void DoDraw(Graphics g, IScreenToCoordinateMapper mapper)
        {
            var keys = (from k in layers.Keys select k).ToList();
            keys.Sort();

            foreach (var key in keys)
            {
                DrawLayer(key, g, mapper);
            }
        }

        protected override void DoUpdate(object data)
        {
            
        }
    }
}
