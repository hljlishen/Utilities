using Unity;
using Unity.Interception;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.Interceptors.TypeInterceptors.VirtualMethodInterception;
using IniBind.InterfaceInterception;
using IniBind.AttributeInterception;
using Unity.Lifetime;

namespace IniBind
{
    public class IniFileObjectFactory<T>
    {
        private static readonly IUnityContainer container;
        static IniFileObjectFactory()
        {
            container = new UnityContainer();
            container.AddNewExtension<Interception>();
        }

        public static T CreateObject()
        {
            if (typeof(IIniBindInterface).IsAssignableFrom(typeof(T)))
            {
                if (!container.IsRegistered<T>())
                {
                    container.RegisterType<T>(new SingletonLifetimeManager(), new Interceptor<VirtualMethodInterceptor>(), new InterceptionBehavior<BindIniBehavior>());
                    container.RegisterType<BindStrategy, InterfaceStrategy>();
                }
            }
            else
            {
                if (!container.IsRegistered<T>())
                {
                    container.RegisterType<T>(new SingletonLifetimeManager(), new Interceptor<VirtualMethodInterceptor>(), new InterceptionBehavior<BindIniBehavior>());
                    container.RegisterType<BindStrategy, AttributeStrategy>();
                }
            }
            return container.Resolve<T>();
        }
    }
}
