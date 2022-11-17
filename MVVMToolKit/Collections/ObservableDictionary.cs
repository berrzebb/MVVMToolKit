using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace MVVMToolKit.Collections
{
    [Serializable]
    public class ObservableKeyValuePair<TKey, TValue> : INotifyPropertyChanged
    {
        #region properties
        private TKey key;
        private TValue value;

        public TKey Key
        {
            get { return this.key; }
            set
            {
                this.key = value;
                this.OnPropertyChanged("Key");
            }
        }

        public TValue Value
        {
            get { return this.value; }
            set
            {
                this.value = value;
                this.OnPropertyChanged("Value");
            }
        }
        #endregion

        #region INotifyPropertyChanged Members

        [field: NonSerialized]
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler? handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion
    }

    [Serializable]
    public class ObservableDictionary<TKey, TValue> : ObservableCollection<ObservableKeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>
    {

        #region IDictionary<TKey,TValue> Members

        public void Add(TKey key, TValue value)
        {
            if (this.ContainsKey(key))
            {
                throw new ArgumentException("The dictionary already contains the key");
            }
            base.Add(new ObservableKeyValuePair<TKey, TValue>() { Key = key, Value = value });
        }

        public bool ContainsKey(TKey key)
        {
            //var m=base.FirstOrDefault((i) => i.Key == key);
            var r = this.ThisAsCollection().FirstOrDefault((i) => this.Equals(key, i.Key));

            return !this.Equals(default(ObservableKeyValuePair<TKey, TValue>), r);
        }

        private bool Equals<TKey>(TKey a, TKey b)
        {
            return EqualityComparer<TKey>.Default.Equals(a, b);
        }

        private ObservableCollection<ObservableKeyValuePair<TKey, TValue>> ThisAsCollection()
        {
            return this;
        }

        public ICollection<TKey> Keys
        {
            get { return (from i in this.ThisAsCollection() select i.Key).ToList(); }
        }

        public bool Remove(TKey key)
        {
            var remove = this.ThisAsCollection().Where(pair => this.Equals(key, pair.Key)).ToList();
            foreach (var pair in remove)
            {
                this.ThisAsCollection().Remove(pair);
            }
            return remove.Count > 0;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default(TValue);
            var r = this.GetKvpByTheKey(key);
            if (this.Equals(r, default(ObservableKeyValuePair<TKey, TValue>)))
            {
                return false;
            }
            value = r.Value;
            return true;
        }

        private ObservableKeyValuePair<TKey, TValue> GetKvpByTheKey(TKey key)
        {
            return this.ThisAsCollection().FirstOrDefault((i) => i.Key.Equals(key));
        }

        public ICollection<TValue> Values
        {
            get { return (from i in this.ThisAsCollection() select i.Value).ToList(); }
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue result;
                if (!this.TryGetValue(key, out result))
                {
                    throw new ArgumentException("Key not found");
                }
                return result;
            }
            set
            {
                if (this.ContainsKey(key))
                {
                    this.GetKvpByTheKey(key).Value = value;
                }
                else
                {
                    this.Add(key, value);
                }
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item.Key, item.Value);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            var r = this.GetKvpByTheKey(item.Key);
            if (this.Equals(r, default(ObservableKeyValuePair<TKey, TValue>)))
            {
                return false;
            }
            return this.Equals(r.Value, item.Value);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            var r = this.GetKvpByTheKey(item.Key);
            if (this.Equals(r, default(ObservableKeyValuePair<TKey, TValue>)))
            {
                return false;
            }
            if (!this.Equals(r.Value, item.Value))
            {
                return false;
            }
            return this.ThisAsCollection().Remove(r);
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        public new IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return (from i in this.ThisAsCollection() select new KeyValuePair<TKey, TValue>(i.Key, i.Value)).ToList().GetEnumerator();
        }

        #endregion
    }
}
