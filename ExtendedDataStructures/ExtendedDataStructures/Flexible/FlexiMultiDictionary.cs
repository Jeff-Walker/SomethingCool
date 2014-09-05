using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ExtendedDataStructures.Flexible {
// ReSharper disable once PossibleInterfaceMemberAmbiguity
    public interface IFlexiMultiDictionary<TKey, TValue, TCollection> 
            : IDictionary<TKey, TCollection>, ILookup<TKey, TValue>
            where TCollection : IEnumerable<TValue>
    {
        void Add(TKey key, TValue value);
        void Add(TKey key, params TValue[] values);
        void Add(TKey key, IEnumerable<TValue> values);
        bool Contains(TKey key, TValue value);
    }

    public abstract class FlexiMultiDictionary<TKey, TValue, TCollection>
            : IFlexiMultiDictionary<TKey, TValue, TCollection>
            where TCollection : ICollection<TValue>
    {
        private readonly IDictionary<TKey, TCollection> _backingDictionary = new Dictionary<TKey, TCollection>();

        protected abstract TCollection ProvideNewCollection();

        // reference implementation. Override if your ICollection choice does this better
        protected virtual void BulkAdd(TCollection valueCollection, IEnumerable<TValue> toAdd) {
            foreach (var v in toAdd) {
                valueCollection.Add(v);
            }
        }

        public virtual bool Contains(TKey key, TValue value) {
            if (_backingDictionary.ContainsKey(key)) {
                var valueCollection = GetValueCollection(key);
                return valueCollection.Contains(value);
            }
            return false;
        }

        public void Add(TKey key, TValue value) {
            var valueCollection = GetValueCollection(key);
            valueCollection.Add(value);
        }

        public void Add(TKey key, params TValue[] values) {
            if (values == null) {
                throw new ArgumentNullException("values","collection can't be null");
            }
            var valueCollection = GetValueCollection(key);
            BulkAdd(valueCollection, values);
        }

        public void Add(TKey key, IEnumerable<TValue> values) {
            if (values == null) {
                throw new ArgumentNullException("values", "collection can't be null");
            }
            var valueCollection = GetValueCollection(key);
            BulkAdd(valueCollection, values);
        }

        void IDictionary<TKey, TCollection>.Add(TKey key, TCollection values) {
            var valueCollection = GetValueCollection(key);
            BulkAdd(valueCollection, values);
        }

        protected TCollection GetValueCollection(TKey key) {
            TCollection collection;
            if (!_backingDictionary.TryGetValue(key, out collection)) {
                collection = ProvideNewCollection();
                _backingDictionary.Add(key, collection);
            }
            return collection;
        }

        // ILookup

        IEnumerator<IGrouping<TKey, TValue>> IEnumerable<IGrouping<TKey, TValue>>.GetEnumerator() {
            IEnumerable<KeyValuePair<TKey, TCollection>> asIEnumerable = _backingDictionary;
            return asIEnumerable.Select(x => new KeyValuePairGrouping<TKey, TValue, TCollection>(x)).GetEnumerator();
        }

        bool ILookup<TKey, TValue>.Contains(TKey key) {
            return _backingDictionary.ContainsKey(key);
        }

        IEnumerable<TValue> ILookup<TKey, TValue>.this[TKey key] {
            get { return _backingDictionary[key]; }
        }

        // IDictionary
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public ICollection<TCollection> Values {
            get { return _backingDictionary.Values; }
        }

        public ICollection<TKey> Keys {
            get { return _backingDictionary.Keys; }
        }

        public TCollection this[TKey key] {
            get { return _backingDictionary[key]; }
            set { _backingDictionary[key] = value; }
        }

        public bool TryGetValue(TKey key, out TCollection value) {
            return _backingDictionary.TryGetValue(key, out value);
        }

        public bool Remove(TKey key) {
            return _backingDictionary.Remove(key);
        }

      

        public bool ContainsKey(TKey key) {
            return _backingDictionary.ContainsKey(key);
        }

        public bool IsReadOnly {
            get { return _backingDictionary.IsReadOnly; }
        }

        public int Count {
            get { return _backingDictionary.Count; }
        }

        public bool Remove(KeyValuePair<TKey, TCollection> item) {
            return _backingDictionary.Remove(item);
        }

        public void CopyTo(KeyValuePair<TKey, TCollection>[] array, int arrayIndex) {
            _backingDictionary.CopyTo(array, arrayIndex);
        }

        public bool Contains(KeyValuePair<TKey, TCollection> item) {
            return _backingDictionary.Contains(item);
        }

        public void Clear() {
            _backingDictionary.Clear();
        }

        public void Add(KeyValuePair<TKey, TCollection> item) {
            _backingDictionary.Add(item);
        }

        public IEnumerator<KeyValuePair<TKey, TCollection>> GetEnumerator() {
            return _backingDictionary.GetEnumerator();
        }
    }

    public class KeyValuePairGrouping<TK, TV, TC> : IGrouping<TK, TV> where TC : IEnumerable<TV> {
        readonly KeyValuePair<TK, TC> _kvp;

        public KeyValuePairGrouping(KeyValuePair<TK, TC> kvp) {
            _kvp = kvp;
        }

        public IEnumerator<TV> GetEnumerator() {
            return _kvp.Value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public TK Key { get { return _kvp.Key; } }
    }
}
