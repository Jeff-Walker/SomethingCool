using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ExtendedDataStructures.MultiDictionary {
    //     ReSharper disable once PossibleInterfaceMemberAmbiguity
    public interface IMultiDictionary<TK, TV> : IDictionary<TK, ICollection<TV>>, ILookup<TK, TV> {
        bool TryGetValue(TK key, out IEnumerable<TV> value);
        void Add(TK key, TV value);
    }

    public class MultiDictionary<TK, TV> : IMultiDictionary<TK, TV> {
        readonly Dictionary<TK, ICollection<TV>> _backingDictionary;

        public MultiDictionary() {
            _backingDictionary = new Dictionary<TK, ICollection<TV>>();
        }

        public bool TryGetValue(TK key, out IEnumerable<TV> value) {
            ICollection<TV> temp;
            var got = _backingDictionary.TryGetValue(key, out temp);
            value = temp;
            return got;
        }

        public void Add(TK key, TV value) {
            GetValueList(key).Add(value);
        }

        ICollection<TV> GetValueList(TK key) {
            ICollection<TV> result;
            if (!_backingDictionary.TryGetValue(key, out result)) {
                result = new List<TV>();
                _backingDictionary.Add(key, result);
            }
            return result;
        }

        public IEnumerator<KeyValuePair<TK, ICollection<TV>>> GetEnumerator() {
            return _backingDictionary.GetEnumerator();
        }

        IEnumerator<IGrouping<TK, TV>> IEnumerable<IGrouping<TK, TV>>.GetEnumerator() {
            IEnumerable<KeyValuePair<TK, ICollection<TV>>> asIEnumerable = _backingDictionary;
            return asIEnumerable.Select(x => new KeyValuePairGrouping<TK, TV>(x)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return _backingDictionary.GetEnumerator();
        }

        public bool Contains(TK key) {
            return _backingDictionary.ContainsKey(key);
        }

        public void Add(KeyValuePair<TK, ICollection<TV>> item) {
            ((ICollection<KeyValuePair<TK, ICollection<TV>>>)_backingDictionary).Add(item);
        }

        public void Clear() {
            _backingDictionary.Clear();
        }

        public bool Contains(KeyValuePair<TK, ICollection<TV>> item) {
            return _backingDictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<TK, ICollection<TV>>[] array, int arrayIndex) {
            ((ICollection<KeyValuePair<TK, ICollection<TV>>>)_backingDictionary).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TK, ICollection<TV>> item) {
            return ((ICollection<KeyValuePair<TK, ICollection<TV>>>)_backingDictionary).Remove(item);
        }

        public int Count { get { return _backingDictionary.Count; } }

        public bool IsReadOnly {
            get { return ((ICollection<KeyValuePair<TK, ICollection<TV>>>)_backingDictionary).IsReadOnly; }
        }

        public bool ContainsKey(TK key) {
            return _backingDictionary.ContainsKey(key);
        }

        public void Add(TK key, ICollection<TV> value) {
            _backingDictionary.Add(key, value);
        }

        public bool Remove(TK key) {
            return _backingDictionary.Remove(key);
        }

        public bool TryGetValue(TK key, out ICollection<TV> value) {
            return _backingDictionary.TryGetValue(key, out value);
        }

        ICollection<TV> IDictionary<TK, ICollection<TV>>.this[TK key] {
            get { return _backingDictionary[key]; }
            set { _backingDictionary[key] = value; }
        }

        public ICollection<TK> Keys {
            get { return _backingDictionary.Keys; }
        }

        public ICollection<ICollection<TV>> Values {
            get { return _backingDictionary.Values; }
        }

        public IEnumerable<TV> this[TK key] {
            get { return _backingDictionary[key]; }
        }

    }



    public class KeyValuePairGrouping<TK, TV> : IGrouping<TK, TV> {
        readonly KeyValuePair<TK, ICollection<TV>> _kvp;

        public KeyValuePairGrouping(KeyValuePair<TK, ICollection<TV>> kvp) {
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
