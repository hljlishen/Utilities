using System.Collections.Generic;
using System.Reflection;
using Unity.Interception.PolicyInjection.Pipeline;
using System.Linq;

namespace Utilities.IniBind
{
    public abstract class BindStrategy
    {
        public abstract string GetFilePath(IMethodInvocation input, PropertyInfo property);
        public abstract string GetSection(IMethodInvocation input, PropertyInfo property);
        public abstract string GetKey(IMethodInvocation input, PropertyInfo property);
        public abstract bool IsIniBindProperty(PropertyInfo p);
        public virtual IEnumerable<PropertyInfo> GetIniProperties(IMethodInvocation input)
        {
            return from p in input.Target.GetType().GetProperties()
                   where IsIniBindProperty(p)
                   select p;
        }
        public abstract bool IsIniPropertyCall(IMethodInvocation input);
        public static PropertyInfo GetCalledProperty(IMethodInvocation input) => input.Target.GetType().GetProperty(input.MethodBase.Name.Substring(4));
    }
}
