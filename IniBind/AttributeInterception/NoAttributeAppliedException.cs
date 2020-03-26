using System;
using System.Reflection;

namespace Utilities.IniBind.AttributeInterception
{
    [Serializable]
    public class NoAttributeAppliedException : Exception
    {
        public NoAttributeAppliedException(PropertyInfo p) : base($"<{p.Name}>没有应用<{nameof(IniKeyAttribute)}>属性")
        {
        }
    }
}
