using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Utilities.Mapper;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.Display
{
    public abstract class MouseSensitiveElement<T> : DynamicElement<MarkerModel> where T : MouseSensitiveObject
    {
        protected List<T> objects = new List<T>();
        public MarkerModel Model { get; protected set; }
        protected Brush normalLineBrush;
        protected Brush selectedLineBrush;
        protected TextFormat normalTextFormat;
        protected TextFormat selectedTextFormat;
        protected Brush normalTextBrush;
        protected Brush selectedTextBrush;

        protected MouseSensitiveElement(MarkerModel model)
        {
            Model = model;
        }

        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            normalLineBrush?.Dispose();
            selectedLineBrush?.Dispose();
            normalTextFormat?.Dispose();
            selectedTextFormat?.Dispose();
            normalTextBrush?.Dispose();
            selectedTextBrush?.Dispose();

            normalLineBrush = Model.LineColor.SolidBrush(rt);
            selectedLineBrush = Model.SelectedLineColor.SolidBrush(rt);
            normalTextBrush = Model.FontColor.SolidBrush(rt);
            selectedTextBrush = Model.SelectedFontColor.SolidBrush(rt);
            DWriteFactory dw = DWriteFactory.CreateFactory();
            normalTextFormat = dw.CreateTextFormat(Model.FontName, Model.FontSize);
            selectedTextFormat = dw.CreateTextFormat(Model.SelectedFontName, Model.SelectedFontSize);
            dw.Dispose();
        }

        public override void Dispose()
        {
            base.Dispose();
            normalLineBrush?.Dispose();
            selectedLineBrush?.Dispose();
            normalTextFormat?.Dispose();
            selectedTextFormat?.Dispose();
            normalTextBrush?.Dispose();
            selectedTextBrush?.Dispose();
        }
        public override void SetDisplayer(Displayer d)
        {
            base.SetDisplayer(d);
            Panel.MouseMove += Panel_MouseMove;
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
        protected abstract IEnumerable<T> GetObjects();

        protected virtual void Panel_MouseMove(object sender, MouseEventArgs e)
        {
            lock (Locker)
            {
                foreach (var o in objects)
                {
                    if (o.IsMouseNear(e.Location))
                    {
                        o.MouseLocation = e.Location;
                        if (!o.Selected)
                            Changed = true;
                        o.Selected = true;
                    }
                    else
                    {
                        if (o.Selected)
                            Changed = true;
                        o.Selected = false;
                    }
                }
            }
        }

        protected override void DoUpdate(MarkerModel t) => Model = t;
    }
}
