﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Utilities.Display
{
    public abstract class Sensor : IDisposable
    {
        public Displayer Displayer { get; private set; }
        public List<LiveObject> objects { get; private set; } = null;
        public Panel Panel => Displayer.Panel;

        public event Action<Sensor> ObjectStateChanged;
        public void InvokeObjectStateChanged() => ObjectStateChanged?.Invoke(this);

        protected object locker = null;

        public void SetLocker(object locker) => this.locker = locker ?? throw new NullReferenceException("locker为空");

        public virtual void SetDisplayer(Displayer d)
        {
            Displayer = d;
            Displayer.BeforeRebindTarget += Displayer_BeforeRebindTarget;
            Displayer.AfterRebindTarget += Displayer_AfterRebindTarget;
            BindEvents(Panel);
        }

        private void Displayer_AfterRebindTarget() => BindEvents(Panel);

        protected abstract void BindEvents(Panel panel);

        private void Displayer_BeforeRebindTarget() => UnbindEvents(Panel);

        protected abstract void UnbindEvents(Panel panel);
        public void SetObjects(List<LiveObject> objects) => this.objects = objects;

        public virtual void Dispose() => UnbindEvents(Panel);
    }
}
