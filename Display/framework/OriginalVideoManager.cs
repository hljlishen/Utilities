using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.Mapper;

namespace Utilities.Display
{

    public struct OriginalVideoData
    {
        public double Angle;
        public List<OriginVideoDotProperty> Dots;
    }
    public class OriginalVideoManager : RotatableElement<OriginalVideoData>
    {
        private Dictionary<int, List<OriginalVideoData>> layerMap = new Dictionary<int, List<OriginalVideoData>>();
        private Dictionary<ValueIntervals.ValueInterval, int> intervalMap = new Dictionary<ValueIntervals.ValueInterval, int>();

        /// <summary>
        /// 此控件要占用多个图层，此参数表示要占用图层的个数。占用的图层是此控件所在图层后面consumedLayerCount个。使用Diplayer时不要向这些图层手动添加元素，否则元素会被删除无法显示
        /// </summary>
        /// <param name="consumedLayerCount">要占用的图层个数</param>
        public OriginalVideoManager(uint consumedLayerCount, string rotateDecoratorName = "default") : base(rotateDecoratorName)
        {
            ConsumedLayerCount = consumedLayerCount;
        }

        public override void SetDisplayer(Displayer d)
        {
            base.SetDisplayer(d);

            //初始化字典
            var intervalCoverage = 360.0f / ConsumedLayerCount;
            for (int i = LayerId + 1, j = 0; i < ConsumedLayerCount + LayerId + 1; i++, j++)    //计算LayerId时要从OriginalVideoManager所属的Layer下一个Id算起，否则RefreshLayer时会把OriginalVideoManager从当前的layer清除掉
            {
                layerMap.Add(i, new List<OriginalVideoData>());
                var interval = ValueIntervals.ValueInterval.CloseOpen(j * intervalCoverage, (j + 1) * intervalCoverage);
                intervalMap.Add(interval, i);
                d.Elements.AddLayer(i);
            }

            Mapper.MapperStateChanged += Mapper_MapperStateChanged;   //必须将Mapper转换为PolarRotateDecorator类型才能接收到改变RotateAngle时触发的MapperStateChanged事件，需要重构Mapper的事件结构以解决该问题
        }

        private void Mapper_MapperStateChanged(Mapper.IScreenToCoordinateMapper obj)
        {
            //Mapper状态改变时，需要更新所有图层的原始视频点
            lock (Locker)
            {
                foreach (var layerId in layerMap.Keys)
                {
                    RefreshLayer(layerId);
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            Mapper.MapperStateChanged -= Mapper_MapperStateChanged;
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

        private IEnumerable<OriginalVideoDot> GetLayerDots(int layerId)
        {
            var OriginalVideoDataCollection = layerMap[layerId];
            foreach (var item in OriginalVideoDataCollection)
            {
                foreach (var dotData in item.Dots)
                {
                    var rotatedDotProperty = new OriginVideoDotProperty() { Location = new Coordinates.PolarCoordinate(dotData.Location.Az + RotateAngle, dotData.Location.El, dotData.Location.Dis), Am = dotData.Am };
                    yield return new OriginalVideoDot(rotatedDotProperty);
                }
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

            if (layerId != currentLayerId) //如果当前的方位超出了当前图层覆盖的角度范围，则更新当前图层
            {
                layerMap[layerId].Clear();  //进入新的存储区之前删除该存储区之前的数据，在更新该区域内容的前一刻才删除该区的数据。不能每次更新图层之后就删除，因为图像旋转时候还需要用到之前存储的数据
                RefreshLayer(currentLayerId);
            }
            layerMap[layerId].Add(t); //将点的视图保存到对应的图层列表中
            currentLayerId = layerId;
        }

        private void RefreshLayer(int id)
        {
            var layer = displayer.Elements.GetLayer(id);
            layer.RefreshLayerElements(GetLayerDots(id).ToList());
        }
    }
}
