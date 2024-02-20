using System.Collections.Concurrent;

namespace MVVMToolKit.Navigation.Mapping
{
    using Templates;

    internal class MappingBuilder : IMappingRegistry, IMappingBuilder
    {

        private readonly ConcurrentDictionary<object, IMappingConfiguration> _mappings = new();


        public IMappingRegistry Register(IMappingConfiguration configuration)
        {
            object? key = string.IsNullOrEmpty(configuration.RouteName) ? configuration.ContextType : configuration.RouteName;
            if (key == null) throw new ArgumentException($"[MappingBuilder] Mapping 의 key가 될 수 있는 RouteName이나 ContextType이 존재하지 않습니다. 확인해 주십시오.", nameof(configuration));
            _mappings.GetOrAdd(key, configuration);
            return this;
        }

        private DataTemplate? CreateFromConfiguration(IMappingConfiguration configuration)
        {
            FrameworkElementFactory viewProxyFactory = new(typeof(ViewProxy));
            viewProxyFactory.SetValue(ViewProxy.ViewTypeProperty, configuration.ViewType);
            viewProxyFactory.SetValue(ViewProxy.ViewModeProperty, configuration.ViewMode);
            viewProxyFactory.SetValue(ViewProxy.ViewCacheModeProperty, configuration.CacheMode);
            viewProxyFactory.SetValue(ViewProxy.ViewSelectorProperty, configuration.ViewSelector);
            DataTemplate? dataTemplate = null;
            if (configuration.ContextType == null)
            {
                dataTemplate = new DataTemplate()
                {
                    VisualTree = viewProxyFactory
                };
            }
            else
            {
                dataTemplate = new DataTemplate(configuration.ContextType)
                {
                    VisualTree = viewProxyFactory
                };

            }
            return dataTemplate;
        }

        ResourceDictionary IMappingBuilder.Build()
        {
            ResourceDictionary resources = new();

            foreach (KeyValuePair<object, IMappingConfiguration> mapping in _mappings)
            {
                resources.Add(new DataTemplateKey(mapping.Key), CreateFromConfiguration(mapping.Value));
            }

            return resources;
        }
    }
}
