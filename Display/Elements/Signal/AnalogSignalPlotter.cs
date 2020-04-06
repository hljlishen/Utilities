using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Drawing;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.Display
{
    public class AnalogSignalPlotter : DynamicElement<double[]>
    {
        private Brush signalBrush;
        private double[] data;

        public override void Dispose()
        {
            base.Dispose();
            signalBrush?.Dispose();
        }

        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            signalBrush = rt.CreateSolidColorBrush(Color.GreenYellow.ToColorF());
        }
        protected override void DoUpdate(double[] t) => data = t;

        protected override void DrawDynamicElement(RenderTarget rt)
        {
            if (data == null || data.Length <= 1)
                return;
            for (int i = 0; i < data.Length - 1; i++)
            {
                var x1 = (float)Mapper.GetScreenX(i);
                var y1 = (float)Mapper.GetScreenY(data[i]);
                var x2 = (float)Mapper.GetScreenX(i+1);
                var y2 = (float)Mapper.GetScreenY(data[i+1]);
                rt.DrawLine(new Point2F(x1, y1), new Point2F(x2, y2), signalBrush, 1);
            }
        }
    }
}
