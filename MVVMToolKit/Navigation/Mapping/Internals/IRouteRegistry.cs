namespace MVVMToolKit.Navigation.Mapping.Internals
{
    public interface IRouteRegistry
    {
        DataTemplate? this[DataTemplateKey routeKey] { get; }
    }
}
