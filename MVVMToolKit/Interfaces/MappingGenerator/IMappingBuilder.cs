namespace MVVMToolKit.Interfaces
{
    public interface IMappingBuilder
    {
        IMappingBuilder AddMapping<TViewModel>(IViewConfiguration configuration);
        IMappingBuilder AddMapping(Uri mappingUri);
        IMappingBuilder RemoveMapping(Uri mappingUri);
        IMappingBuilder RemoveMapping<TViewModel>();
        ResourceDictionary Build();
    }
}
