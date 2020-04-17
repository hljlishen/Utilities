using System;
using System.Windows.Forms;
using Utilities.Mapper;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.Display
{
    public abstract class GraphicElement : IDisposable
    {
        public int LayerId { get; set; }
        private bool Changed = true;
        protected Displayer displayer;
        protected readonly object Locker = new object();
        protected Sensor sensor;
        public Panel Panel => displayer.Panel;
        public virtual IScreenToCoordinateMapper Mapper => displayer.Mapper;
        public ReferenceSystem ReferenceSystem => displayer.ReferenceSystem;
        protected abstract void DrawElement(RenderTarget rt);
        protected virtual void InitializeComponents(RenderTarget rt) { }
        public virtual bool HasChanged() => Changed;
        public virtual void UpdateGraphic() => Changed = true;
        public List<LiveObject> Objects { get; protected set; }
        protected bool Initialized { get; set; } = true;

        /// <summary>
        /// 如果元素需要相应鼠标，必须重写此函数。
        /// </summary>
        /// <returns>LiveObject相应鼠标操作的区域</returns>
        protected virtual IEnumerable<LiveObject> GetObjects() => null;
        public virtual Sensor Sensor
        {
            get => sensor;
            set
            {
                sensor?.Dispose();
                sensor = Guards.Guard.NullCheckAssignment(value);
                sensor.SetLocker(Locker);
                sensor.ObjectStateChanged += Sensor_ObjectStateChanged;
            }
        }

        protected virtual void Sensor_ObjectStateChanged(Sensor obj) => UpdateGraphic();

        protected void RefreshObjects()
        {
            lock (Locker)
            {
                Objects?.Clear();
                Objects = GetObjects()?.ToList();
                Sensor?.SetObjects(Objects);
            }
        }

        /// <summary>
        /// 框架负责调用次函数，用户不要主动调用
        /// </summary>
        /// <param name="d">显示器</param>
        public virtual void SetDisplayer(Displayer d)
        {
            displayer = d;
            Sensor?.SetDisplayer(d);
            RefreshObjects();
            Mapper.MapperStateChanged += Mapper_MapperStateChanged;
            displayer.BeforeRebindTarget += Displayer_BeforeRebindTarget;
            displayer.AfterRebindTarget += Displayer_AfterRebindTarget;
            BindEvents(Panel);
        }

        private void Displayer_AfterRebindTarget()
        {
            BindEvents(Panel);
            Initialized = true;
            Changed = true;
        }

        private void Displayer_BeforeRebindTarget() => UnbindEvents(Panel);
        protected virtual void BindEvents(Panel p) { }
        protected virtual void UnbindEvents(Panel p) { }

        public void Draw(RenderTarget rt)
        {
            if (Initialized)
            {
                InitializeComponents(rt);
                Initialized = false;
            }
            DrawElement(rt);
            Changed = false;
        }

        private void Mapper_MapperStateChanged(IScreenToCoordinateMapper obj) => RefreshObjects();
        public virtual void Dispose()
        {
            Sensor?.Dispose();
            UnbindEvents(Panel);
        }
    }
}
