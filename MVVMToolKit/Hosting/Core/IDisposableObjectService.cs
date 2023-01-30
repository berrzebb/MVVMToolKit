namespace MVVMToolKit.Hosting.Core
{
    /// <summary>
    /// The disposable object service interface
    /// </summary>
    /// <seealso cref="IDisposable"/>
    public interface IDisposableObjectService : IDisposable
    {
        /// <summary>
        /// Adds the disposable.
        /// </summary>
        /// <param name="disposable">The disposable.</param>
        void Add(IDisposableObject disposable);

        /// <summary>
        /// Describes whether this instance exists.
        /// </summary>
        /// <param name="guid">Disposable Guid Key</param>
        /// <returns>Exists Guids Result.</returns>
        bool Exists(Guid guid);

        /// <summary>
        /// Removes the disposable.
        /// </summary>
        /// <param name="disposable">The disposable.</param>
        void Remove(IDisposableObject disposable);
    }
}
