using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MVVMToolKit.MVVM
{
    public class NotifyObject<T> : INotifyPropertyChanged, INotifyPropertyChanging where T : new()
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event PropertyChangingEventHandler? PropertyChanging;

        private T _value;

        public NotifyObject(
#pragma warning disable CS8601 // Nullable
        T initialValue = default
#pragma warning restore CS8601 // Nullable
            )
        {
            this._value = initialValue;
        }

        public T Value
        {
            get { return this._value; }
            set {
                if (!EqualityComparer<T>.Default.Equals(this._value, value)) {
                    this.OnPropertyChanging(nameof(this.Value));
                    this._value = value;
                    this.OnPropertyChanged(nameof(this.Value));
                }
            }
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            ArgumentNullException.ThrowIfNull(e);

            PropertyChanged?.Invoke(this, e);
        }
        protected virtual void OnPropertyChanging(PropertyChangingEventArgs e)
        {
            ArgumentNullException.ThrowIfNull(e);


            PropertyChanging?.Invoke(this, e);
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertyChanging([CallerMemberName] string ? propertyName = null)
        {
            this.OnPropertyChanging(new PropertyChangingEventArgs(propertyName));
        }
    }
}
