using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ExtendedDataStructures.Flexible {
// ReSharper disable once PossibleInterfaceMemberAmbiguity
    public interface IFlexiMultiDictionary<TK, TV, TC> : IDictionary<TK, TC>, ILookup<TK, TV> where TC : IEnumerable<TV> {
        void Add(TK key, TV value);
        void Add(TK key, params TV[] values);
        void Add(TK key, IEnumerable<TV> values);
        bool Contains(TK key, TV value);
    }

    public abstract class FlexiMultiDictionary<TK, TV, TC> : IFlexiMultiDictionary<TK, TV, TC> where TC : ICollection<TV> {
        private readonly IDictionary<TK, TC> _backingDictionary = new Dictionary<TK, TC>();

        protected abstract TC ProvideNewCollection();

        // reference implementation. Override if your ICollection choice does this better
        protected virtual void BulkAdd(TC valueCollection, IEnumerable<TV> toAdd) {
            foreach (var v in toAdd) {
                valueCollection.Add(v);
            }
        }

        public virtual bool Contains(TK key, TV value) {
            if (_backingDictionary.ContainsKey(key)) {
                var valueCollection = GetValueCollection(key);
                return valueCollection.Contains(value);
            }
            return false;
        }

        public void Add(TK key, TV value) {
            var valueCollection = GetValueCollection(key);
            valueCollection.Add(value);
        }

        public void Add(TK key, params TV[] values) {
            if (values == null) {
                throw new ArgumentNullException("values","collection can't be null");
            }
            var valueCollection = GetValueCollection(key);
            BulkAdd(valueCollection, values);
        }

        public void Add(TK key, IEnumerable<TV> values) {
            if (values == null) {
                throw new ArgumentNullException("values", "collection can't be null");
            }
            var valueCollection = GetValueCollection(key);
            BulkAdd(valueCollection, values);
        }

        void IDictionary<TK, TC>.Add(TK key, TC values) {
            var valueCollection = GetValueCollection(key);
            BulkAdd(valueCollection, values);
        }

        protected TC GetValueCollection(TK key) {
            TC collection;
            if (!_backingDictionary.TryGetValue(key, out collection)) {
                collection = ProvideNewCollection();
                _backingDictionary.Add(key, collection);
            }
            return collection;
        }

        // ILookup

        IEnumerator<IGrouping<TK, TV>> IEnumerable<IGrouping<TK, TV>>.GetEnumerator() {
            IEnumerable<KeyValuePair<TK, TC>> asIEnumerable = _backingDictionary;
            return asIEnumerable.Select(x => new KeyValuePairGrouping<TK, TV, TC>(x)).GetEnumerator();
        }

        bool ILookup<TK, TV>.Contains(TK key) {
            return _backingDictionary.ContainsKey(key);
        }

        IEnumerable<TV> ILookup<TK, TV>.this[TK key] {
            get { return _backingDictionary[key]; }
        }

        // IDictionary
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public ICollection<TC> Values {
            get { return _backingDictionary.Values; }
        }

        public ICollection<TK> Keys {
            get { return _backingDictionary.Keys; }
        }

        public TC this[TK key] {
            get { return _backingDictionary[key]; }
            set { _backingDictionary[key] = value; }
        }

        public bool TryGetValue(TK key, out TC value) {
            return _backingDictionary.TryGetValue(key, out value);
        }

        public bool Remove(TK key) {
            return _backingDictionary.Remove(key);
        }

      

        public bool ContainsKey(TK key) {
            return _backingDictionary.ContainsKey(key);
        }

        public bool IsReadOnly {
            get { return _backingDictionary.IsReadOnly; }
        }

        public int Count {
            get { return _backingDictionary.Count; }
        }

        public bool Remove(KeyValuePair<TK, TC> item) {
            return _backingDictionary.Remove(item);
        }

        public void CopyTo(KeyValuePair<TK, TC>[] array, int arrayIndex) {
            _backingDictionary.CopyTo(array, arrayIndex);
        }

        public bool Contains(KeyValuePair<TK, TC> item) {
            return _backingDictionary.Contains(item);
        }

        public void Clear() {
            _backingDictionary.Clear();
        }

        public void Add(KeyValuePair<TK, TC> item) {
            _backingDictionary.Add(item);
        }

        public IEnumerator<KeyValuePair<TK, TC>> GetEnumerator() {
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
