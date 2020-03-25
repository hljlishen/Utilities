using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Unity.Interception.PolicyInjection.Pipeline;

namespace IniBind.AttributeInterception
{
    class AttributeStrategy : BindStrategy
    {
        public override IEnumerable<PropertyInfo> GetIniProperties(IMethodInvocation input)
        {
            var property = GetCalledProperty(input);
            var attri = GetIniAttribute(property);
            return base.GetIniProperties(input).Where(p=>GetIniAttribute(p).File == attri.File);
        }
        public override string GetFilePath(IMethodInvocation input, PropertyInfo property) => Path.GetFullPath(GetIniAttribute(property).File);
        private IniKeyAttribute GetIniAttribute(PropertyInfo property)
        {
            var attribute = property.GetCustomAttribute(typeof(IniKeyAttribute));

            if (attribute == null)
            {
                throw new NoAttributeAppliedException(property);
            }
            return attribute as IniKeyAttribute;
        }

        public override string GetKey(IMethodInvocation input, PropertyInfo property) => GetIniAttribute(property).Key;

        public override string GetSection(IMethodInvocation input, PropertyInfo property) => GetIniAttribute(property).Section;

        public override bool IsIniBindProperty(PropertyInfo p) => p.GetCustomAttribute(typeof(IniKeyAttribute)) != null && p.GetAccessors()[0].IsVirtual;

        public override bool IsIniPropertyCall(IMethodInvocation input)
        {
            var methodName = input.MethodBase.Name;
            if (!methodName.StartsWith("set_") && !methodName.StartsWith("get_"))
                return false;

            var propertyName = methodName.Substring(4);
            var pInfo = input.Target.GetType().GetProperty(propertyName);
            return IsIniBindProperty(pInfo);
        }
    }
}
