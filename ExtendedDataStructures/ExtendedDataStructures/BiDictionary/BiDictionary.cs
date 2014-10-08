using System;
using System.Collections;
using System.Collections.Generic;

namespace ExtendedDataStructures.BiDictionary {

    /// <summary>
    ///  A basic implementation of an <code>IBiDictionary</code> based on two 
    /// <code>Dictionary</code> objects.
    /// </summary>
    /// <see cref="IBiDictionary{TK,TV}"/>
    /// <see cref="IDictionary{TKey,TValue}"/>
    /// <typeparam name="TK"></typeparam>
    /// <typeparam name="TV"></typeparam>
    public class BiDictionary<TK, TV> : AbstractBiDict<TK, TV> {
        private IBiDictionary<TV, TK> _inverse;

        public BiDictionary() : base(new Dictionary<TK, TV>(), new Dictionary<TV, TK>()) {}
        public BiDictionary(int capacity)
            : base(new Dictionary<TK, TV>(capacity), new Dictionary<TV, TK>(capacity)) { }
        public BiDictionary(IEqualityComparer<TK> kComparer, IEqualityComparer<TV> vComparer)
            : base(new Dictionary<TK, TV>(kComparer), new Dictionary<TV, TK>(vComparer)) { }
        public BiDictionary(int capacity, IEqualityComparer<TK> kComparer, IEqualityComparer<TV> vComparer)
            : base(new Dictionary<TK, TV>(capacity, kComparer), new Dictionary<TV, TK>(capacity, vComparer)) { }

        protected BiDictionary(IDictionary<TK, TV> forward, IDictionary<TV, TK> backward)
            : base(forward, backward) {}

        public override IBiDictionary<TV, TK> Inverse {
            get { return _inverse ?? (_inverse = new BiDictionary<TV, TK>(Backward, Forward) {Inverse = this}); }
            protected set { _inverse = value; }
        }
    }

    public abstract class AbstractBiDict<TK, TV> : IBiDictionary<TK, TV> {
        protected readonly IDictionary<TK, TV> Forward;
        protected readonly IDictionary<TV, TK> Backward;
        private const string ValueAlreadyMappedMessage = "Value is already mapped to a different key.";

        public abstract IBiDictionary<TV, TK> Inverse { get; protected set; }

        protected AbstractBiDict(IDictionary<TK, TV> forward, IDictionary<TV,TK> backward ) {
            Forward = forward;
            Backward = backward;
        }

        public void ForceAdd(TK key, TV value) {
            AddItem(key, value, true);
        }

        protected virtual bool AddItem(TK key, TV value, bool force) {
            var valueAlreadyMapped = ValueAlreadyMapped(key, value);
            if (valueAlreadyMapped) {
                if (!force) {
                    return false;
                }
                Inverse.Remove(value);
            }
            Forward.Add(key, value);
            Backward.Add(value, key);
            return true;
        }

        protected virtual bool AddItem(KeyValuePair<TK, TV> item, bool force) {
            var valueAlreadyMapped = ValueAlreadyMapped(item.Key, item.Value);
            if (!force && !valueAlreadyMapped) {
                return false;
            }
            if (valueAlreadyMapped) {
                Inverse.Remove(item.Value);
            }
            Forward.Add(item);
            Backward.Add(new KeyValuePair<TV, TK>(item.Value, item.Key));
            return true;
        }

        private bool ValueAlreadyMapped(TK key, TV value) {
            TK otherKey;
            var mapped = Backward.TryGetValue(value, out otherKey);
            return mapped && !Equals(otherKey, key);
        }

        public IEnumerator<KeyValuePair<TK, TV>> GetEnumerator() {
            return Forward.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable) Forward).GetEnumerator();
        }

        public void Add(KeyValuePair<TK, TV> item) {
            var added = AddItem(item, false);
            if (!added) {
                throw new ArgumentException(ValueAlreadyMappedMessage);
            }
        }

        public void Clear() {
            Forward.Clear();
            Backward.Clear();
        }

        public bool Contains(KeyValuePair<TK, TV> item) {
            return Forward.Contains(item);
        }

        public void CopyTo(KeyValuePair<TK, TV>[] array, int arrayIndex) {
            Forward.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TK, TV> item) {
            Backward.Remove(new KeyValuePair<TV, TK>(item.Value, item.Key));
            return Forward.Remove(item);
        }

        public int Count {
            get { return Forward.Count; }
        }

        public bool IsReadOnly {
            get { return Forward.IsReadOnly; }
        }

        public bool ContainsKey(TK key) {
            return Forward.ContainsKey(key);
        }

        public bool ContainsValue(TV value) {
            return Backward.ContainsKey(value);
        }

        public void Add(TK key, TV value) {
            var added = AddItem(key, value, false);
            if (!added) {
                throw new ArgumentException(ValueAlreadyMappedMessage);
            }
        }

        public bool Remove(TK key) {
            TV value;
            var present = Forward.TryGetValue(key, out value);
            if (!present) {
                return false;
            }
            Backward.Remove(value);
            return Forward.Remove(key);
        }

        public bool TryGetValue(TK key, out TV value) {
            return Forward.TryGetValue(key, out value);
        }

        public TV this[TK key] {
            get { return Forward[key]; }
            set { Add(key, value); }
        }

        public ICollection<TK> Keys {
            get { return Forward.Keys; }
        }

        public ICollection<TV> Values {
            get { return Forward.Values; }
        }

    }
}