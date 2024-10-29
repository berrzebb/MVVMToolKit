using System.Collections.Concurrent;
using System.Windows;
using MVVMToolKit.Navigation.Mapping.Internals;
using MVVMToolKit.Templates;

namespace MVVMToolKit.Navigation.Mapping
{
    internal class MappingManager : IMappingRegistry, IMappingBuilder, IRouteRegistry
    {

        private readonly ConcurrentDictionary<object, IMappingConfiguration> _mappings = new();
        private readonly ConcurrentDictionary<DataTemplateKey, DataTemplate?> _routes = new();

        public DataTemplate? this[DataTemplateKey routeKey] => _routes.TryGetValue(routeKey, out DataTemplate? route) ? route : null;

        public IMappingRegistry Register(IMappingConfiguration configuration)
        {
            object? key = string.IsNullOrEmpty(configuration.RouteName) ? configuration.ContextType : configuration.RouteName;
            if (key == null) throw new ArgumentException($"[MappingManager] Mapping 의 key가 될 수 있는 RouteName이나 ContextType이 존재하지 않습니다. 확인해 주십시오.", nameof(configuration));
            _mappings.GetOrAdd(key, configuration);
            return this;
        }

        internal static DataTemplate? CreateFromConfiguration(IMappingConfiguration configuration)
        {
            FrameworkElementFactory viewProxyFactory = new(typeof(ViewProxy));
            viewProxyFactory.SetValue(ViewProxy.ViewNameProperty, configuration.ViewName);
            viewProxyFactory.SetValue(ViewProxy.ViewCacheModeProperty, configuration.CacheMode);
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
            ResourceDictionary resourceDictionary = new();

            foreach (KeyValuePair<object, IMappingConfiguration> mapping in _mappings)
            {
                DataTemplateKey routeKey = new(mapping.Key);
                DataTemplate? dataTemplate = CreateFromConfiguration(mapping.Value);
                resourceDictionary.Add(routeKey, dataTemplate);
                _routes.TryAdd(routeKey, dataTemplate);
            }

            return resourceDictionary;
        }
    }
}
