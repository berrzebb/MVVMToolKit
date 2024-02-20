using System.Collections.Concurrent;

namespace MVVMToolKit.Interfaces.ViewMapper
{
    internal class MappingBuilder : IMappingBuilder
    {
        private readonly ConcurrentDictionary<Type, IViewConfiguration> _mappings = new();
        private readonly ConcurrentDictionary<Uri, ResourceDictionary> _resources = new();
        /// <inheritdoc />
        public IMappingBuilder AddMapping<TViewModel>(IViewConfiguration configuration)
        {
            _mappings.GetOrAdd(typeof(TViewModel), configuration);
            return this;
        }

        private ResourceDictionary CreateFromUri(Uri mappingUri) => new() { Source = mappingUri };

        private DataTemplate CreateFromConfiguration(Type viewModel, IViewConfiguration configuration)
        {
            var viewProxyFactory = new FrameworkElementFactory(typeof(ViewProxy));
            viewProxyFactory.SetValue(ViewProxy.ViewTypeProperty, configuration.ViewType);
            viewProxyFactory.SetValue(ViewProxy.ViewModeProperty, configuration.ViewMode);
            viewProxyFactory.SetValue(ViewProxy.ViewCacheModeProperty, configuration.CacheMode);
            viewProxyFactory.SetValue(ViewProxy.ViewSelectorProperty, configuration.ViewSelector);
            var dataTemplate = new DataTemplate(viewModel)
            {
                VisualTree = viewProxyFactory
            };
            return dataTemplate;
        }
        public IMappingBuilder AddMapping(Uri mappingUri)
        {
            _resources.GetOrAdd(mappingUri, CreateFromUri);
            return this;
        }

        public IMappingBuilder RemoveMapping(Uri mappingUri)
        {

            _resources.TryRemove(mappingUri, out _);

            return this;
        }

        public IMappingBuilder RemoveMapping<TViewModel>()
        {
            _mappings.TryRemove(typeof(TViewModel), out _);

            return this;
        }
        /// <inheritdoc />
        public ResourceDictionary Build()
        {
            ResourceDictionary resources = new ResourceDictionary();
            foreach (var resource in _resources.Values)
            {
                resources.MergedDictionaries.Add(resource);
            }
            foreach (var mapping in _mappings)
            {

                resources.Add(new DataTemplateKey(mapping.Key), CreateFromConfiguration(mapping.Key, mapping.Value));
            }
            return resources;
        }
    }
}
