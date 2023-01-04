namespace MVVMToolKit.Hosting.Core
{
    /// <summary>
    /// The disposable object interface.
    /// </summary>
    /// <seealso cref="IDisposable"/>
    public interface IDisposableObject : IDisposable
    {
        /// <summary>
        /// Gets or sets the value of the guid.
        /// </summary>
        Guid Guid { get; set; }
    }
}
