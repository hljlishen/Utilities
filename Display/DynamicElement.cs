﻿using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;

namespace Utilities.Display
{
    public abstract class DynamicElement<T> : GraphicElement
    {
        protected override void DrawElement(RenderTarget rt)
        {
            lock (Locker)
            {
                DrawDynamicElement(rt);
            }
        }

        protected abstract void DrawDynamicElement(RenderTarget rt);

        public virtual void Update(T t)
        {
            lock(Locker)
            {
                DoUpdate(t);
                Changed = true;
            }
        }

        protected abstract void DoUpdate(T t);
    }
}
