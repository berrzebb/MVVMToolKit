
namespace MVVMToolKit
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// The Notify Object Class.
    /// </summary>
    /// <typeparam name="T">Property Type.</typeparam>
    public class NotifyObject<T> : INotifyPropertyChanged, INotifyPropertyChanging
        where T : new()
    {
        /// <summary>
        /// The PropertyChanged.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// The PropertyChanging.
        /// </summary>
        public event PropertyChangingEventHandler? PropertyChanging;

        /// <summary>
        /// The value.
        /// </summary>
        private T _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyObject{T}"/> class.
        /// </summary>
        /// <param name="initialValue">The initial value.</param>
        public NotifyObject(T initialValue = default!)
        {
            _value = initialValue;
        }

        /// <summary>
        /// Gets or sets the value of the value.
        /// </summary>
        public T Value
        {
            get => _value;
            set
            {
                if (!EqualityComparer<T>.Default.Equals(_value, value))
                {
                    OnPropertyChanging();
                    _value = value;
                    OnPropertyChanged();
                }
            }
        }



        /// <summary>
        /// Ons the property changed using the specified property name.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
#if NET6_0
            ArgumentNullException.ThrowIfNull(propertyName);
#endif
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Ons the property changing using the specified property name.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        protected void OnPropertyChanging([CallerMemberName] string? propertyName = null)
        {
#if NET6_0
            ArgumentNullException.ThrowIfNull(propertyName);
#endif
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }
    }
}
