using System.Windows.Markup;

namespace MVVMToolKit.Ioc
{
    /// <summary>
    /// The container provider extension class
    /// </summary>
    /// <seealso cref="MarkupExtension"/>
    public class ContainerProviderExtension : MarkupExtension
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerProviderExtension"/> class.
        /// </summary>
        public ContainerProviderExtension()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerProviderExtension"/> class.
        /// </summary>
        /// <param name="type">The type to Resolve</param>
        /// <param name="serviceKey">Service Key(Optional)</param>
        public ContainerProviderExtension(Type? type, object? serviceKey)
        {
            Type = type;
            ServiceKey = serviceKey;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerProviderExtension"/> class.
        /// </summary>
        /// <param name="type">The type to Resolve</param>
        /// <param name="serviceKey">Service Key(Optional)</param>
        public ContainerProviderExtension(Type? type) : this(type, default)
        {
        }
        /// <summary>
        /// The type to Resolve
        /// </summary>
        public Type? Type { get; }
        /// <summary>
        /// The type to Resolve
        /// </summary>
        public object? ServiceKey { get; }

        /// <summary>
        /// Provide resolved object from <see cref="ContainerLocator"/>
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object? ProvideValue(IServiceProvider serviceProvider)
        {
            return ContainerProvider.Resolve(Type, ServiceKey);
        }
    }
}
