using System.Collections;
using System.Collections.Generic;

namespace ExtendedDataStructures.Forwarding {
    
    public abstract class ForwardingDictionary<TK, TV> : IDictionary<TK, TV> {

        protected abstract IDictionary<TK, TV> Delegate { get; }

        public IEnumerator<KeyValuePair<TK, TV>> GetEnumerator() {
            return Delegate.GetEnumerator();
        }

        public void Add(KeyValuePair<TK, TV> item) {
            Delegate.Add(item);
        }

        public void Clear() {
            Delegate.Clear();
        }

        public bool Contains(KeyValuePair<TK, TV> item) {
            return Delegate.Contains(item);
        }

        public void CopyTo(KeyValuePair<TK, TV>[] array, int arrayIndex) {
            Delegate.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TK, TV> item) {
            return Delegate.Remove(item);
        }

        public int Count {
            get { return Delegate.Count; }
        }

        public bool IsReadOnly {
            get { return Delegate.IsReadOnly; }
        }

        public bool ContainsKey(TK key) {
            return Delegate.ContainsKey(key);
        }

        public void Add(TK key, TV value) {
            Delegate.Add(key, value);
        }

        public bool Remove(TK key) {
            return Delegate.Remove(key);
        }

        public bool TryGetValue(TK key, out TV value) {
            return Delegate.TryGetValue(key, out value);
        }

        public TV this[TK key] {
            get { return Delegate[key]; }
            set { Delegate[key] = value; }
        }

        public ICollection<TK> Keys {
            get { return Delegate.Keys; }
        }

        public ICollection<TV> Values {
            get { return Delegate.Values; }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}