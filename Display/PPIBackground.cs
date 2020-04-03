using System;
using System.Drawing;

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

        public override void Dispose()
        {
        }

        public override void Draw(Graphics graphics)
        {
            base.Draw(graphics);
            var disStep = Range / (MarkerCount + 1);
            var center = Mapper.GetScreenLocation(0, 0);

            for (int i = 1; i <= MarkerCount + 1; i++)
            {
                var dis = disStep * i;
                var x = Mapper.GetScreenX(dis);
                var r = Math.Abs(x - center.X);
                var rect = new RectangleF((float)(center.X - r), (float)(center.Y - r), (float)(r * 2), (float)(r * 2));
                graphics.DrawEllipse(new Pen(Color.Green, 1), rect);

                graphics.DrawString(dis.ToString("0.0"), new Font("宋体", 12), new SolidBrush(Color.Gray), new Point((int)center.X, (int)(center.Y - r + 5)));
            }

            var p = new Pen(Color.White, 1);
            graphics.DrawLine(p, Mapper.GetScreenLocation(-Range, 0), Mapper.GetScreenLocation(Range, 0));
            graphics.DrawLine(p, Mapper.GetScreenLocation(0, Range), Mapper.GetScreenLocation(0, -Range));
        }
    }
}
