//using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
//using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//namespace Utilities.Display
//{
//    public class Chart_Bar : MouseMoveElement<LiveRect, Dictionary<string, double>>
//    {
//        public Dictionary<string, double> Data { get; private set; } = new Dictionary<string, double>();

//        protected override void DoUpdate(Dictionary<string, double> t)
//        {
//            if (Data.Count > 0)
//                Data?.Clear();
//            foreach (var item in t)     //深拷贝数据
//            {
//                Data.Add(item.Key, item.Value);
//            }
//            base.DoUpdate(t);
//        }

//        protected override IEnumerable<LiveRect> GetObjects()
//        {
//            if (Data.Count == 0)
//                return new List<LiveRect>();

//            return DoGetObjects();

//            IEnumerable<LiveRect> DoGetObjects()
//            {
//                var step = ReferenceSystem.XDistance / (Data.Count * 1.2);

//                int i = 1;
//                var yBottom = Mapper.GetScreenY(0);
//                foreach (var item in Data)
//                {
//                    var xCoo = ReferenceSystem.Left + step * (i++);
//                    var xScr = Mapper.GetScreenX(xCoo);
//                    var yTop = Mapper.GetScreenY(item.Value);
//                    yield return new LiveRect(new RectangleF((float)xScr - 15, Math.Min((float)yTop, (float)yBottom), 30, Math.Abs((float)yTop - (float)yBottom))) { Value = item.Key};
//                }
//            }
//        }

//        protected override void DrawObjectSelected(RenderTarget rt, LiveRect r)
//        {
//            var brush = Color.Orange.SolidBrush(rt);
//            var frameBrush = Color.Green.SolidBrush(rt);
//            rt.FillRectangle(r.Rectangle.ToRectF(), brush);
//            rt.DrawRectangle(r.Rectangle.ToRectF(), frameBrush, 2);
//            brush.Dispose();
//            frameBrush.Dispose();

//            var textBrush = Color.White.SolidBrush(rt);
//            DWriteFactory dw = DWriteFactory.CreateFactory();
//            TextFormat normalTextFormat = dw.CreateTextFormat("微软雅黑", 20);
//            if (r.Rectangle.Bottom > Mapper.GetScreenY(0))
//            {
//                rt.DrawText(r.Value.ToString(), normalTextFormat, new RectangleF(r.Rectangle.X - 5, r.Rectangle.Top - 40, 100, 100).ToRectF(), textBrush);
//            }
//            else
//            {
//                rt.DrawText(r.Value.ToString(), normalTextFormat, new RectangleF(r.Rectangle.X - 5, r.Rectangle.Bottom + 20, 100, 100).ToRectF(), textBrush);
//            }
//            dw.Dispose();
//            textBrush.Dispose();
//            normalTextFormat.Dispose();
//        }

//        protected override void DrawObjectUnselected(RenderTarget rt, LiveRect r)
//        {
//            var brush = Color.Pink.SolidBrush(rt);
//            var frameBrush = Color.White.SolidBrush(rt);
//            rt.FillRectangle(r.Rectangle.ToRectF(), brush);
//            rt.DrawRectangle(r.Rectangle.ToRectF(), frameBrush, 1);
//            brush.Dispose();
//            frameBrush.Dispose();

//            var textBrush = Color.White.SolidBrush(rt);
//            DWriteFactory dw = DWriteFactory.CreateFactory();
//            TextFormat normalTextFormat = dw.CreateTextFormat("微软雅黑", 20);
//            if (r.Rectangle.Bottom > Mapper.GetScreenY(0))
//            {
//                rt.DrawText(r.Value.ToString(), normalTextFormat, new RectangleF(r.Rectangle.X - 5, r.Rectangle.Top - 40, 100, 100).ToRectF(), textBrush);
//            }
//            else
//            {
//                rt.DrawText(r.Value.ToString(), normalTextFormat, new RectangleF(r.Rectangle.X - 5, r.Rectangle.Bottom + 20, 100, 100).ToRectF(), textBrush);
//            }
//            dw.Dispose();
//            textBrush.Dispose();
//            normalTextFormat.Dispose();
//        }
//    }
//}
