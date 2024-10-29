using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;
using MVVMToolKit.Interfaces;

namespace MVVMToolKit.Ioc.Modules
{
    /// <inheritdoc />
    public class DirectoryModuleCatalog : ModuleCatalog
    {
        /// <summary>
        /// Directory Module Catalog 생성자.
        /// </summary>
        /// <param name="moduleDirectory">모듈 디렉토리</param>
        public DirectoryModuleCatalog(string moduleDirectory)
        {


            var moduleDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, moduleDirectory);
            if (!Directory.Exists(moduleDir)) return;

            Initialize(moduleDir);

        }

        private void ResolveAssembly(string assemblyPath)
        {
            Assembly? assembly = null;
            try
            {
                assembly = Assembly.Load(File.ReadAllBytes(assemblyPath));
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"[ModuleCatalog] {assemblyPath} Load Exception");
            }

            var modules = assembly?
                .GetExportedTypes()
                .Where(t => t != ModuleType && !t.IsAbstract && ModuleType.IsAssignableFrom(t))
                .Select(t => Activator.CreateInstance(t) as IModule) ?? Enumerable.Empty<IModule>();

            foreach (var module in modules)
            {
                AddModule(module);
            }
        }
        private void Initialize(string moduleDir)
        {
            foreach (string file in Directory.GetFiles(moduleDir))
            {
                //load our newly added assembly
                ResolveAssembly(file);
            }
        }
    }
}
