using System;

namespace IniBind.InterfaceInterception
{
    [Serializable]
    internal class NotIIniBindInterfaceInstanceException : Exception
    {
        public NotIIniBindInterfaceInstanceException(object target) : base($"<{target.GetType().FullName}>未继承<{nameof(IIniBindInterface)}>接口")
        {
        }
    }
}