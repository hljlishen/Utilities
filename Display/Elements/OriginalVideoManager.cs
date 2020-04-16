using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.Display
{

    public struct OriginalVideoData
    {
        public double Angle;
        public List<OriginVideoDotProperty> Dots;
    }
    public class OriginalVideoManager : DynamicElement<OriginalVideoData>
    {
        private Dictionary<int, List<OriginalVideoDot>> layerMap = new Dictionary<int, List<OriginalVideoDot>>();
        private Dictionary<ValueIntervals.ValueInterval, int> intervalMap = new Dictionary<ValueIntervals.ValueInterval, int>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="consumedLayerCount">此控件要占用多个图层，此参数表示要占用图层的个数。占用的图层是此控件所在图层后面consumedLayerCount个。使用Diplayer时不要向这些图层手动添加元素，否则元素会被删除无法显示</param>
        public OriginalVideoManager(uint consumedLayerCount)
        {
            ConsumedLayerCount = consumedLayerCount;
        }

        public override void SetDisplayer(Displayer d)
        {
            base.SetDisplayer(d);

            //初始化字典
            var intervalCoverage = 360.0f / ConsumedLayerCount;
            for (int i = LayerId, j = 0; i < ConsumedLayerCount + LayerId; i++, j++)
            {
                layerMap.Add(i, new List<OriginalVideoDot>());
                var interval = ValueIntervals.ValueInterval.CloseOpen(j * intervalCoverage, (j + 1) * intervalCoverage);
                intervalMap.Add(interval, i);
                d.Elements.AddLayer(i);
            }
        }

        private int FindLayerId(OriginalVideoData p)
        {
            foreach (var interval in intervalMap.Keys)
            {
                if (interval.IsInRange(p.Angle))
                {
                    return intervalMap[interval];
                }
            }
            throw new Exception("找不到OriginVideoDotProperty所在的图层");
        }

        private IEnumerable<OriginalVideoDot> GetDots(OriginalVideoData p)
        {
            foreach(var d in p.Dots)
            {
                yield return new OriginalVideoDot(d);
            }
        }

        public uint ConsumedLayerCount { get; private set; }
        private int currentLayerId;
        private bool firstTime = true;
        protected override void DrawDynamicElement(RenderTarget rt)
        {
            //do nothing
        }

        protected override void DoUpdate(OriginalVideoData t)
        {
            base.DoUpdate(t);   //将t保存到Model中，Model在此类中的含义是，最新一个OriginalVideoDot的值
            var layerId = FindLayerId(t);
            if(firstTime)
            {
                currentLayerId = layerId;
                firstTime = false;
            }

            layerMap[layerId].AddRange(GetDots(t).ToList()); //将点的视图保存到对应的图层列表中

            if (layerId != currentLayerId) //如果当前的方位超出了当前图层覆盖的角度范围，则更新当前图层
            {
                var layer = displayer.Elements.GetLayer(currentLayerId);
                layer.RefreshLayerElements(layerMap[currentLayerId]);
                layerMap[currentLayerId].Clear();
            }
            currentLayerId = layerId;
        }
    }
}
