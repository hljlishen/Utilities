using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Utilities.Mapper;

namespace Utilities.Display
{
    public abstract class MouseSensitiveElement<ObjectType, UpdateType> : DynamicElement<UpdateType> where ObjectType : LiveObject
    {
        protected List<ObjectType> objects = new List<ObjectType>();

        protected MouseSensitiveElement()
        {
        }

        public override void SetDisplayer(Displayer d)
        {
            base.SetDisplayer(d);
            Mapper.MapperStateChanged += Mapper_MapperStateChanged;
            objects.Clear();
            objects = GetObjects().ToList();
        }

        private void Mapper_MapperStateChanged(IScreenToCoordinateMapper obj)
        {
            lock (Locker)
            {
                objects.Clear();
                objects = GetObjects().ToList();
            }
        }
        protected abstract IEnumerable<ObjectType> GetObjects();

        protected virtual void ProcessMouseEvent(MouseEventArgs e)
        {
            lock (Locker)
            {
                foreach (var o in objects)
                {
                    if (o.IsPointNear(e.Location))
                    {
                        o.MouseLocation = e.Location;
                        if (!o.Selected)
                            UpdateGraphic();
                        o.Selected = true;
                    }
                    else
                    {
                        if (o.Selected)
                            UpdateGraphic();
                        o.Selected = false;
                    }
                }
            }
        }

        protected override void DoUpdate(UpdateType t)
        {
            objects.Clear();
            objects = GetObjects().ToList();
        }

        protected override void DrawDynamicElement(RenderTarget rt)
        {
            foreach (var o in objects)
            {
                if (o.Selected)
                    DrawObjectSelected(rt, o);
                else
                    DrawObjectUnselected(rt, o);
            }
        }

        protected abstract void DrawObjectUnselected(RenderTarget rt, ObjectType o);
        protected abstract void DrawObjectSelected(RenderTarget rt, ObjectType o);
    }
}
