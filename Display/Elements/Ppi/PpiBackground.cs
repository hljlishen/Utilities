using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
using System;
using Utilities.Tools;
using System.Drawing;

namespace Utilities.Display
{
    public class PpiBackground : Background
    {
        public PpiBackground(double range = 100)
        {
            Range = range;
        }

        public PpiBackground(BackgroundModel model)
        {
            if (Math.Abs(model.XLeft) != Math.Abs(model.XRight) || Math.Abs(model.YTop) != Math.Abs(model.YBottom))
                throw new BackgroundModelDoesntFillPpi();
            Range = Math.Abs(model.XLeft);
        }

        public double Range 
        { 
            get => XRight;
            set
            {
                XLeft = -value;
                XRight = value;
                YTop = value;
                YBottom = -value;
            } 
        }
        public uint MarkerCount { get; set; } = 5;
        public TextFormat distanceMarkTextFormat { get; private set; }
        public SolidColorBrush textBrush { get; private set; } = null;
        public SolidColorBrush antennaBrush { get; private set; } = null;

        public override void Dispose()
        {
            distanceMarkTextFormat?.Dispose();
            textBrush?.Dispose();
            antennaBrush?.Dispose();
        }

        protected override void DrawDynamicElement(RenderTarget rt)
        {
            base.DrawDynamicElement(rt);
            var disStep = Range / (MarkerCount + 1);
            var center = Mapper.GetScreenLocation(0, 0);

            for (int i = 1; i <= MarkerCount + 1; i++)
            {
                var dis = disStep * i;
                var x = Mapper.GetScreenX(dis);
                var r = Math.Abs(x - center.X);
                Ellipse e = new Ellipse(new Point2F(center.X, center.Y), (float)r, (float)r);
                rt.DrawEllipse(e, rt.CreateSolidColorBrush(new ColorF(0, 255, 0)), 2);

                RectF rect = new RectF(center.X, center.Y - (float)r, center.X + 80, center.Y - (float)r + 80);
                rt.DrawText(dis.ToString("0.0"), distanceMarkTextFormat, rect, textBrush);
            }
            rt.DrawLine(Mapper.GetScreenLocation(-Range, 0).ToPoint2F(), Mapper.GetScreenLocation(Range, 0).ToPoint2F(), antennaBrush, 2);
            rt.DrawLine(Mapper.GetScreenLocation(0, Range).ToPoint2F(), Mapper.GetScreenLocation(0, -Range).ToPoint2F(), antennaBrush, 2);
        }

        protected override void InitializeComponents(RenderTarget rt)
        {
            textBrush = rt.CreateSolidColorBrush(new ColorF(new ColorI(128, 138, 135)));
            DWriteFactory dw = DWriteFactory.CreateFactory();
            distanceMarkTextFormat = dw.CreateTextFormat("Berlin Sans FB Demi", 15);
            antennaBrush = rt.CreateSolidColorBrush(Functions.GetColorFFromRgb(255, 255, 255));
            dw.Dispose();
        }
    }

    public class BackgroundModelDoesntFillPpi : Exception
    {
        public BackgroundModelDoesntFillPpi() : base("ppi背景要求XLeft和XRight绝对值相等；YTop和YBottom绝对值相等")
        {
        }
    }
}
