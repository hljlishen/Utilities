using System.IO;
using System.Reflection;
using Unity.Interception.PolicyInjection.Pipeline;

namespace IniBind.InterfaceInterception
{
    class InterfaceStrategy : BindStrategy
    {
        public override string GetFilePath(IMethodInvocation input, PropertyInfo property)
        {
            var target = input.Target;
            AssertType(target);
            return Path.GetFullPath((target as IIniBindInterface).FilePath);
        }

        public override string GetKey(IMethodInvocation input, PropertyInfo p)
        {
            AssertType(input.Target);

            return p.Name;
        }

        public override string GetSection(IMethodInvocation input, PropertyInfo p)
        {
            AssertType(input.Target);
            return (input.Target as IIniBindInterface).Section;
        }

        public override bool IsIniBindProperty(PropertyInfo p) => p.Name.EndsWith("_ini") && p.GetAccessors()[0].IsVirtual;

        public override bool IsIniPropertyCall(IMethodInvocation input)
        {
            var methodName = input.MethodBase.Name;
            return (methodName.StartsWith("set_") || methodName.StartsWith("get_")) && methodName.EndsWith("_ini");
        }

        private void AssertType(object target)
        {
            if (!(target is IIniBindInterface))
                throw new NotIIniBindInterfaceInstanceException(target);
        }
    }
}
