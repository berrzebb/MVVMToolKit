using System.Reflection;
using Microsoft.Extensions.Logging;
using MVVMToolKit.Interfaces;

namespace MVVMToolKit.Ioc.Modules
{
    /// <summary>
    /// 모듈을 관리하기 위한 카탈로그 객체
    /// </summary>
    public class ModuleCatalog : List<IModule>, IModuleCatalog
    {
        /// <summary>
        /// 로깅 객체
        /// </summary>
        protected readonly ILogger<ModuleCatalog>? Logger = ContainerProvider.Resolve<ILogger<ModuleCatalog>>();

        /// <summary>
        /// 모듈을 찾기 위한 타입(IModule)
        /// </summary>
        protected readonly Type ModuleType;

        /// <summary>
        /// 모듈 카탈로그 정적 생성자.
        /// </summary>
        public ModuleCatalog()
        {
            Assembly moduleAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .First(asm => asm.FullName == typeof(IModule).Assembly.FullName);
            ModuleType = moduleAssembly.GetType(typeof(IModule).FullName!)!;
        }
        /// <summary>
        /// 모듈 목록
        /// </summary>
        public IEnumerable<IModule> Modules => this;
        /// <inheritdoc/>
        public IModuleCatalog AddModule(IModule? module)
        {
            if (module == null) return this;
            Add(module);
            Logger?.LogInformation($"[ModuleCatalog] {module.GetType().Name} Module Added");
            return this;
        }
        /// <inheritdoc/>
        public IModuleCatalog AddModule(Type moduleType) => AddModule((IModule?)Activator.CreateInstance(moduleType));
        /// <inheritdoc/>
        public IModuleCatalog AddModule<TModule>() where TModule : IModule => AddModule(typeof(TModule));
    }
}
