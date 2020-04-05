using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
using System.Drawing;
using Utilities.Coordinates;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.Display
{
    public class RectangularLocationElement : DynamicElement<PointF>
    {
        private PointF currentCoordinateLocation;
        private PointF currentScreenLcation;
        private Brush textBrush;
        public TextFormat distanceMarkTextFormat;

        public override void Dispose()
        {
            base.Dispose();
            textBrush?.Dispose();
            distanceMarkTextFormat?.Dispose();
        }
        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            textBrush = rt.CreateSolidColorBrush(Color.AntiqueWhite.ToColorF());
            DWriteFactory dw = DWriteFactory.CreateFactory();
            distanceMarkTextFormat = dw.CreateTextFormat("Berlin Sans FB Demi", 25);
            dw.Dispose();
        }

        public override void SetDisplayer(Displayer d)
        {
            base.SetDisplayer(d);
            Panel.MouseMove += Panel_MouseMove;
        }

        private void Panel_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Update(e.Location);
        }

        protected override void DoDraw(RenderTarget rt)
        {
            //RectF r = new RectF(currentScreenLcation.X, currentScreenLcation.Y - 40, currentScreenLcation.X + 300, currentScreenLcation.Y + 60);
            RectF r = new RectF(0, 0, 300, 60);
            rt.DrawText($"CX:{currentCoordinateLocation.X} ,CY:{currentCoordinateLocation.Y}", distanceMarkTextFormat, r, textBrush);
            r = new RectF(0, 60, 300, 120);
            rt.DrawText($"SX:{currentScreenLcation.X} ,SY:{currentScreenLcation.Y}", distanceMarkTextFormat, r, textBrush);
        }

        protected override void DoUpdate(PointF t)
        {
            currentScreenLcation = t;
            currentCoordinateLocation = Mapper.GetCoordinateLocation(t.X, t.Y);
        }
    }
}
