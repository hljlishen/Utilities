﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Utilities.Display
{
    public class LayeredElement : GraphicElement
    {
        protected Dictionary<int, Layer> layers = new Dictionary<int, Layer>();

        public override void Draw(Graphics g)
        {
            lock (Locker)
            {
                var keys = (from k in layers.Keys select k).ToList();
                keys.Sort();

                foreach (var key in keys)
                {
                    layers[key].Draw(g);
                }
                base.Draw(g);
            }
        }

        public void DrawChangedLayers(Graphics g)
        {
            lock (Locker)
            {
                var keys = (from k in layers.Keys select k).ToList();
                keys.Sort();

                foreach (var key in keys)
                {
                    layers[key].DrawIfChanged(g);
                }
            }
        }

        public void AddElement(int layerId, GraphicElement e)
        {
            lock(Locker)
            {
                if (!layers.ContainsKey(layerId))
                {
                    Layer layer = new Layer(layerId);
                    layer.SetDisplayer(displayer);
                    layers[layerId] = layer;
                }
                layers[layerId].AddElement(e);
                Changed = true;
            }
        }

        public override void Dispose()
        {
            lock (Locker)
            {
                foreach (var key in layers.Keys)
                {
                    layers[key].Dispose();
                }
            }
        }
    }
}
