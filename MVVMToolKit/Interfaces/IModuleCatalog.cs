namespace MVVMToolKit.Interfaces
{
    public interface IModuleCatalog
    {
        void AddModule(IModule? module);
        IEnumerable<IModule> Modules { get; }
    }
}
