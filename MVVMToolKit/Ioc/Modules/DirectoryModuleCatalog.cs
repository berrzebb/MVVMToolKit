namespace MVVMToolKit.Ioc.Modules
{
    using System.IO;
    using System.Reflection;
    using Interfaces;
    using Microsoft.Extensions.Logging;

    public class DirectoryModuleCatalog : ModuleCatalog
    {

        public DirectoryModuleCatalog(string moduleDirectory)
        {


            var moduleDir = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, moduleDirectory);
            if (!Directory.Exists(moduleDir)) return;

            this.Initialize(moduleDir);

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
                this.Logger?.LogError(ex, $"[ModuleCatalog] {assemblyPath} Load Exception");
            }

            var modules = assembly?
                .GetExportedTypes()
                .Where(t => t != ModuleType && !t.IsAbstract && ModuleType.IsAssignableFrom(t))
                .Select(t => Activator.CreateInstance(t) as IModule) ?? Enumerable.Empty<IModule>();

            foreach (var module in modules)
            {
                this.AddModule(module);
            }
        }
        private void Initialize(string moduleDir)
        {
            foreach (string file in Directory.GetFiles(moduleDir))
            {
                //load our newly added assembly
                this.ResolveAssembly(file);
            }
        }
    }
}
