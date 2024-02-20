namespace MVVMToolKit.Navigation.Mapping
{
    public interface IMappingRegistry
    {
        IMappingRegistry Register(IMappingConfiguration configuration);
    }
    internal interface IMappingBuilder
    {
        ResourceDictionary Build();
    }
}
