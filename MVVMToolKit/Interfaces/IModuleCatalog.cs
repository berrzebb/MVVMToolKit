namespace MVVMToolKit.Interfaces
{
    public interface IModuleCatalog : IEnumerable<IModule>
    {
        IModuleCatalog AddModule(IModule? module);
        IModuleCatalog AddModule(Type moduleType);
        IModuleCatalog AddModule<TModule>() where TModule : IModule;
    }
}
