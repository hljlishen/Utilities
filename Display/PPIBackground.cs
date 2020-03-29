using System;
using System.Drawing;
using System.Windows.Forms;
using Utilities.Mapper;

namespace Utilities.Display
{
    public class PPIBackground : Background
    {
        public PPIBackground(double range = 100)
        {
            Range = range;
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

        protected override void DoDraw(Graphics graphics, IScreenToCoordinateMapper mapper)
        {
            base.Draw(graphics, mapper);
            var disStep = Range / (MarkerCount + 1);
            var center = mapper.ScreenCenter;

            for (int i = 1; i <= MarkerCount + 1; i++)
            {
                var dis = disStep * i;
                var x = mapper.GetScreenX(dis);
                var r = Math.Abs(x - center.X);
                var rect = new RectangleF((float)(center.X - r), (float)(center.Y - r), (float)(r * 2), (float)(r * 2));
                graphics.DrawEllipse(new Pen(Color.Green, 1), rect);

                graphics.DrawString(dis.ToString("0.0"), new Font("宋体", 12), new SolidBrush(Color.Gray), new Point((int)center.X, (int)(center.Y - r + 5)));
            }

            var p = new Pen(Color.White, 1);
            graphics.DrawLine(p, new Point((int)mapper.ScreenLeft, (int)center.Y), new Point((int)mapper.ScreenRight, (int)center.Y));
            graphics.DrawLine(p, new Point((int)center.X, (int)mapper.ScreenTop), new Point((int)center.X, (int)mapper.ScreenBottom));
        }
    }
}
