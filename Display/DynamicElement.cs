namespace Utilities.Display
{
    public abstract class DynamicElement<T> : ThreadSafeElement
    {
        public void Update(T data)
        {
            lock(Locker)
            {
                DoUpdate(data);
                Changed = true;
            }
        }

        protected abstract void DoUpdate(T data);
    }
}
