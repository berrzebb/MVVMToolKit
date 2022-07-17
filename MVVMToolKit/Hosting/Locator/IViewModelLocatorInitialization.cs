using MVVMToolKit.Hosting.Core;

namespace MVVMToolKit.Hosting.Locator
{
    public interface IViewModelLocatorInitialization<in TViewModelLocator>
        : IApplicationInitialize
    {
        /// <summary>
        /// Pre initialization that happens after <see cref="IApplicationInitialize.Initialize"/>. This action happens on UI thread.
        /// This method should be used to set <see cref="AbstractViewModelLocatorHost{TViewModelLocator}.SetViewModelLocator"/>
        /// </summary>
        void InitializeLocator(TViewModelLocator viewModelLocator);
    }
}
