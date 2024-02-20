
namespace MVVMToolKit.Interfaces
{
    using System.Reflection;
    using Ioc;
    using Microsoft.Extensions.Logging;

    public class ModuleCatalog : IModuleCatalog
    {
        protected ILogger<ModuleCatalog>? Logger = ContainerProvider.Resolve<ILogger<ModuleCatalog>>();

        protected static readonly Type ModuleType;
        static ModuleCatalog()
        {
            Assembly moduleAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .First(asm => asm.FullName == typeof(IModule).Assembly.FullName);
            ModuleType = moduleAssembly.GetType(typeof(IModule).FullName!)!;
        }
        private readonly List<IModule> _modules = new();
        public IEnumerable<IModule> Modules => _modules;

        public void AddModule(IModule? module)
        {
            if (module == null) return;
            _modules.Add(module);
            this.Logger?.LogInformation($"[ModuleCatalog] {module.GetType().Name} Module Added");
        }
    }
}
