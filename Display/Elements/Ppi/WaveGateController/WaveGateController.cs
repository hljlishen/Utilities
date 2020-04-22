using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Utilities.Coordinates;
using Utilities.Mapper;
using Utilities.Tools;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;
namespace Utilities.Display
{
    public struct WaveGate
    {
        public uint Id;
        public double BeginAngle;
        public double EndAngle;
        public double BeginDistance;
        public double EndDistance;
    }

    struct WaveGateCoordinates
    {
        public uint Id;
        public RectangularCoordinate P1;
        public RectangularCoordinate P2;
    }

    //public class WaveGateController : MouseClickToCancelSelectionElement<LiveSectorRing, int>, ISwtichable
    public class WaveGateController : RotatableElement<int>, ISwtichable
    {
        private uint currentWaveGateId = 0;
        private readonly Dictionary<LiveObject, WaveGateCoordinates> waveGateMap = new Dictionary<LiveObject, WaveGateCoordinates>();
        private WaveGateSelector selector;
        private Brush frameBrush, normalFillBrush, selectedFillBrush;

        public WaveGateController(string rotateDecoratotInstanceName = "default") : base(rotateDecoratotInstanceName)
        {
        }

        public bool IsOn => selector.IsOn;

        public string Name { get; set; } = "波门选择";

        public event Action<WaveGateController, WaveGate> WaveGateSelected;
        public event Action<uint> WaveGateDeleted;

        public override void Dispose()
        {
            base.Dispose();
            selector.SelectionFinish -= Selector_SelectionFinish;
            selector.Dispose();
            frameBrush.Dispose();
            normalFillBrush.Dispose();
            selectedFillBrush.Dispose();
        }

        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            frameBrush = Color.White.SolidBrush(rt);
            normalFillBrush = Color.Yellow.SolidBrush(rt);
            normalFillBrush.Opacity = 0.5f;
            selectedFillBrush = Color.Orange.SolidBrush(rt);
            selectedFillBrush.Opacity = 0.5f;
        }

        public void DeleteSelectedWaveGates()
        {
            lock (Locker)
            {
                if (Objects.Count == 0)
                    return;
                for (int i = Objects.Count - 1; i >= 0; i--)
                {
                    var sr = Objects[i];
                    if (sr.Selected)
                    {
                        uint id = waveGateMap[sr].Id;
                        waveGateMap.Remove(sr);
                        WaveGateDeleted?.Invoke(id);
                        Objects.Remove(sr);
                    }
                }
                UpdateView();
            }
        }

        public override void SetDisplayer(Displayer d)
        {
            base.SetDisplayer(d);
            selector = new WaveGateSelector();
            d.Elements.Add(LayerId, selector);
            selector.SelectionFinish += Selector_SelectionFinish;
        }


        private void Selector_SelectionFinish(PointF arg1, PointF arg2)
        {
            lock (Locker)
            {
                var pp1 = Mapper.GetCoordinateLocation(arg1.X, arg1.Y).ToRectangularCoordinate().Polar;
                var pp2 = Mapper.GetCoordinateLocation(arg2.X, arg2.Y).ToRectangularCoordinate().Polar;
                var r1 = Mapper.GetCoordinateLocation(arg1.X, arg1.Y).ToRectangularCoordinate();
                var r2 = Mapper.GetCoordinateLocation(arg2.X, arg2.Y).ToRectangularCoordinate();
                WaveGateCoordinates wgc = new WaveGateCoordinates() { Id = currentWaveGateId++, P1 = r1, P2 = r2 };

                PolarCoordinate p1 = new PolarCoordinate(r1);
                PolarCoordinate p2 = new PolarCoordinate(r2);
                var begin = Functions.FindSmallArcBeginAngle(p1.Az, p2.Az) - (Mapper as PolarRotateDecorator).RotateAngle;
                var end = Functions.FindSmallArcEndAngle(p1.Az, p2.Az) - (Mapper as PolarRotateDecorator).RotateAngle;
                WaveGate w = new WaveGate() { BeginAngle = begin, EndAngle = end, BeginDistance = Math.Min(p1.Dis, p2.Dis), EndDistance = Math.Max(p1.Dis, p2.Dis), Id = currentWaveGateId };

                WaveGateSelected?.Invoke(this, w);
                LiveSectorRing ring = new LiveSectorRing() { Center = ReferenceSystem.ScreenOriginalPoint, ScrP1 = arg1, ScrP2 = arg2 };
                Objects.Add(ring);

                waveGateMap.Add(ring, wgc);

                UpdateView();
            }
        }

        protected void DrawObjectSelected(RenderTarget rt, LiveSectorRing o)
        {
            var geo = WaveGateSelector.GetPathGeometry(rt, o.Center, o.ScrP1, o.ScrP2);
            rt.DrawGeometry(geo, frameBrush, 3);
            rt.FillGeometry(geo, selectedFillBrush);
        }

        protected void DrawObjectUnselected(RenderTarget rt, LiveSectorRing o)
        {
            var geo = WaveGateSelector.GetPathGeometry(rt, o.Center, o.ScrP1, o.ScrP2);
            rt.DrawGeometry(geo, frameBrush, 3);
            rt.FillGeometry(geo, normalFillBrush);
        }

        protected override IEnumerable<LiveObject> GetObjects()
        {
            var oPoint = ReferenceSystem.ScreenOriginalPoint;

            //此处需要将waveGateMap清空重新添加映射，因为GetObject之后的LiveSectorRing对象是重新计算的，需要用新的LiveSectorRing对象重新建立 LiveSectorRing<-->WaveGateCoordinate映射。
            //此处如果不更新映射，displayer放缩之后再删除波门，会用新的LiveSectorRing对象在旧的LiveSectorRing<-->WaveGateCoordinate映射中查找WaveGateCoordinate，从而抛出异常
            var values = waveGateMap.Values.ToList();
            waveGateMap.Clear();
            foreach (var w in values)
            {
                var scrP1 = Mapper.GetScreenLocation(w.P1.X, w.P1.Y);
                var scrP2 = Mapper.GetScreenLocation(w.P2.X, w.P2.Y);
                LiveSectorRing liveSectorRing = new LiveSectorRing() { ScrP1 = scrP1, Center = oPoint, ScrP2 = scrP2 };
                waveGateMap.Add(liveSectorRing, w);
                yield return liveSectorRing;
            }
        }

        public void On() => selector.On();

        public void Off() => selector.Off();

        protected override void DrawDynamicElement(RenderTarget rt)
        {
            foreach (var o in Objects)
            {
                var l = o as LiveSectorRing;
                if (l.Selected)
                    DrawObjectSelected(rt, l);
                else
                    DrawObjectUnselected(rt, l);
            }
        }
    }
}
