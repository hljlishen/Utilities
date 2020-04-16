using System;

namespace Utilities.Guards
{
    public static class Guard
    {
        public static void AssertNotNull(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException();
        }

        public static T ConvertToType<T>(object o) where T:class
        {
            var ret = o as T;
            if (ret == null)
                throw new Exception($"参数类型错误，需要{typeof(T)}类型的参数");
            return ret;
        }

        public static T NullCheckAssignment<T>(T t) where T : class => t ?? throw new NullReferenceException();
    }
}
