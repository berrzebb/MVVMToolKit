
namespace MVVMToolKit.Ioc.Modules
{
    using System.Reflection;
    using Interfaces;
    using Ioc;
    using Microsoft.Extensions.Logging;


    public class ModuleCatalog : List<IModule>, IModuleCatalog
    {
        protected readonly ILogger<ModuleCatalog>? Logger = ContainerProvider.Resolve<ILogger<ModuleCatalog>>();

        protected static readonly Type ModuleType;

        /// <inheritdoc />
        static ModuleCatalog()
        {
            Assembly moduleAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .First(asm => asm.FullName == typeof(IModule).Assembly.FullName);
            ModuleType = moduleAssembly.GetType(typeof(IModule).FullName!)!;
        }
        public IEnumerable<IModule> Modules => this;

        public IModuleCatalog AddModule(IModule? module)
        {
            if (module == null) return this;
            Add(module);
            Logger?.LogInformation($"[ModuleCatalog] {module.GetType().Name} Module Added");
            return this;
        }
        public IModuleCatalog AddModule(Type moduleType) => AddModule((IModule?)Activator.CreateInstance(moduleType));
        public IModuleCatalog AddModule<TModule>() where TModule : IModule => AddModule(typeof(TModule));
    }
}
