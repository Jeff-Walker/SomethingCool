using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using ExtendedDataStructures.Forwarding;

namespace ExtendedDataStructures.BiDictionary {
    public class BiDictionary<TK,TV> : IBiDictionary<TK,TV> {
        private IDictionary<TK, TV> _forward;
//        private IDictionary<TV, TK> _inverse;
        public IBiDictionary<TV, TK> Inverse() {
            throw new System.NotImplementedException();
        }

        public void ForceAdd(TK key, TV value) {
            throw new System.NotImplementedException();
        }




    }

    internal class AbstractBiDict<TK, TV> : IBiDictionary<TK, TV> {
        private readonly IDictionary<TK, TV> _forward;
        private readonly IDictionary<TV, TK> _backward;
        private const string ValueAlreadyMappedMessage = "Value is already mapped to a different key.";

        public AbstractBiDict(IDictionary<TK, TV> forward, IDictionary<TV,TK> backward ) {
            _forward = forward;
            _backward = backward;
        }

        protected virtual bool AddItem(TK key, TV value, bool force) {
            if (!force && !AlreadyMapped(key, value)) {
                return false;
            }
            _forward.Add(key, value);
            _backward.Add(value, key);
            return true;
        }

        protected virtual bool AddItem(KeyValuePair<TK, TV> item, bool force) {
            if (!force && !AlreadyMapped(item.Key, item.Value)) {
                return false;
            }
            _forward.Add(item);
            _backward.Add(new KeyValuePair<TV, TK>(item.Value, item.Key));
            return true;
        }

        private bool AlreadyMapped(TK key, TV value) {
            TK otherKey;
            var mapped = _backward.TryGetValue(value, out otherKey);
            if (mapped && !Equals(otherKey, key)) {
                return false;
            }
            return true;
        }

        public IEnumerator<KeyValuePair<TK, TV>> GetEnumerator() {
            return _forward.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable) _forward).GetEnumerator();
        }

        public void Add(KeyValuePair<TK, TV> item) {
            var added = AddItem(item, false);
//            var added = AddItem(item.Key, item.Value, false);
            if (!added) {
                throw new ArgumentException(ValueAlreadyMappedMessage);
            }
        }

        public void Clear() {
            _forward.Clear();
            _backward.Clear();
        }

        public bool Contains(KeyValuePair<TK, TV> item) {
            return _forward.Contains(item);
        }

        public void CopyTo(KeyValuePair<TK, TV>[] array, int arrayIndex) {
            _forward.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TK, TV> item) {
            _backward.Remove(new KeyValuePair<TV, TK>(item.Value, item.Key));
            return _forward.Remove(item);
        }

        public int Count {
            get { return _forward.Count; }
        }

        public bool IsReadOnly {
            get { return _forward.IsReadOnly; }
        }

        public bool ContainsKey(TK key) {
            return _forward.ContainsKey(key);
        }

        public void Add(TK key, TV value) {
//            _forward.Add(key, value);
            var added = AddItem(key, value, false);
            if (!added) {
                throw new ArgumentException(ValueAlreadyMappedMessage);
            }
        }

        public bool Remove(TK key) {
            TV value;
            var present = _forward.TryGetValue(key, out value);
            if (!present) {
                return false;
            }
            _backward.Remove(value);
            return _forward.Remove(key);
        }

        public bool TryGetValue(TK key, out TV value) {
            return _forward.TryGetValue(key, out value);
        }

        public TV this[TK key] {
            get { return _forward[key]; }
            set { Add(key, value); }
        }

        public ICollection<TK> Keys {
            get { return _forward.Keys; }
        }

        public ICollection<TV> Values {
            get { return _forward.Values; }
        }
    }
}