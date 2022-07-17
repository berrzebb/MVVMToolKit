using System.Reflection;

namespace MVVMToolKit.Hosting.Core
{
    public interface IBootstrapper<in TContainer> where TContainer : class
    {
        void Boot(TContainer container, Assembly[] assemblies);
    }
}
