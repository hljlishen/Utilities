using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Drawing;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.Display
{
    public class DiscreteSignalPlotter : DynamicElement<double[]>
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
            var yBottom = (float)Mapper.GetScreenY(0);
            if (data == null)
                return;
            for (int i = 0; i < data.Length; i++)
            {
                var x = (float)Mapper.GetScreenX(i);
                var yTop = (float)Mapper.GetScreenY(data[i]);
                rt.DrawLine(new Point2F(x, yTop), new Point2F(x, yBottom), signalBrush, 1);
            }
        }
    }
}
